using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Application.Strategies;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Application.Services;

public class WalletService
{
    private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

    private readonly IWalletRepository _walletRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ICache _cache;
    private readonly ILogger<WalletService> _logger;
    private readonly IReadOnlyDictionary<FundsOperation, IFundsStrategy> _fundsStrategies;

    public WalletService(
        IWalletRepository walletRepository,
        IPlayerRepository playerRepository,
        ICache cache,
        IEnumerable<IFundsStrategy> strategies,
        ILogger<WalletService> logger)
    {
        _walletRepository = walletRepository;
        _playerRepository = playerRepository;
        _cache = cache;
        _logger = logger;
        _fundsStrategies = strategies.ToDictionary(strategy => strategy.Operation);
    }
    private static string WalletKey(int id) => $"wallet:{id}";
    private static string PlayerWalletsKey(int playerId) => $"wallets:player:{playerId}";
    private const string AllWalletsKey = "wallets:all";
    public async Task<Wallet> CreateWalletAsync(int playerId, Currency currency, decimal initialBalance = 0m, CancellationToken cancellationToken = default)
    {
        if (await _playerRepository.GetByIdAsync(playerId, cancellationToken) is null)
            throw new PlayerNotFoundException(playerId);

        var wallet = new Wallet(await GenerateWalletIdAsync(cancellationToken), playerId, currency, initialBalance);
        await _walletRepository.AddAsync(wallet, cancellationToken);
        _logger.LogInformation("Wallet {WalletId} created for player {PlayerId} in {Currency} with balance {Balance}",
            wallet.Id, playerId, currency, initialBalance);
        Refresh(wallet);
        return wallet;
    }
    public async Task<Wallet?> GetByIdAsync(int walletId, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGet(WalletKey(walletId), out Wallet? cached) && cached is not null)
        {
            _logger.LogInformation("Cache HIT  wallet {WalletId}", walletId);
            return cached;
        }

        _logger.LogInformation("Cache MISS wallet {WalletId} — loading from database", walletId);
        var wallet = await _walletRepository.GetByIdAsync(walletId, cancellationToken);
        if (wallet is not null)
            _cache.Set(WalletKey(walletId), wallet, Ttl);
        return wallet;
    }
    public async Task<IReadOnlyList<Wallet>> GetByPlayerAsync(int playerId, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGet(PlayerWalletsKey(playerId), out IReadOnlyList<Wallet>? cached) && cached is not null)
        {
            _logger.LogInformation("Cache HIT  wallets of player {PlayerId}", playerId);
            return cached;
        }

        _logger.LogInformation("Cache MISS wallets of player {PlayerId} — loading from database", playerId);
        var wallets = await _walletRepository.GetByPlayerAsync(playerId, cancellationToken);
        _cache.Set(PlayerWalletsKey(playerId), wallets, Ttl);
        return wallets;
    }
    public async Task<IReadOnlyList<Wallet>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGet(AllWalletsKey, out IReadOnlyList<Wallet>? cached) && cached is not null)
        {
            _logger.LogInformation("Cache HIT  all wallets");
            return cached;
        }
        _logger.LogInformation("Cache MISS all wallets — loading from database");
        var wallets = await _walletRepository.GetAllAsync(cancellationToken);
        _cache.Set(AllWalletsKey, wallets, Ttl);
        return wallets;
    }
    public async Task<Wallet?> DepositAsync(int walletId, decimal amount, CancellationToken cancellationToken = default)
    {
        var wallet = await _walletRepository.GetByIdAsync(walletId, cancellationToken);
        if (wallet is null)
            return null;

        wallet.Deposit(amount);
        await _walletRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Deposited {Amount} to wallet {WalletId}; new balance {Balance}",
            amount, walletId, wallet.Balance);
        Refresh(wallet);
        return wallet;
    }
    public async Task<Wallet> DepositAsync(int playerId, Currency currency, decimal amount, CancellationToken cancellationToken = default)
    {
        var wallet = await GetTrackedWalletAsync(playerId, currency, cancellationToken);
        wallet.Deposit(amount);
        await _walletRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Deposited {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})",
            amount, playerId, currency, wallet.Balance);
        Refresh(wallet);
        return wallet;
    }
    public async Task<Wallet> WithdrawAsync(int playerId, Currency currency, decimal amount, CancellationToken cancellationToken = default)
    {
        var wallet = await GetTrackedWalletAsync(playerId, currency, cancellationToken);
        wallet.Withdraw(amount);
        await _walletRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})",
            amount, playerId, currency, wallet.Balance);
        Refresh(wallet);
        return wallet;
    }
    public async Task<Wallet> SetBlockedAsync(int playerId, Currency currency, bool blocked, CancellationToken cancellationToken = default)
    {
        var wallet = await GetTrackedWalletAsync(playerId, currency, cancellationToken);
        if (blocked)
            wallet.Block();
        else
            wallet.Unblock();

        await _walletRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Player {PlayerId} {Currency} wallet is now {State}",
            playerId, currency, blocked ? "blocked" : "active");
        Refresh(wallet);
        return wallet;
    }
    public async Task<Wallet> UpdateBalanceAsync(int playerId, Currency currency, decimal newBalance, CancellationToken cancellationToken = default)
    {
        var wallet = await GetTrackedWalletAsync(playerId, currency, cancellationToken);
        wallet.SetBalance(newBalance);
        await _walletRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Player {PlayerId} {Currency} wallet balance set to {Balance}",
            playerId, currency, newBalance);
        Refresh(wallet);
        return wallet;
    }
    public async Task<Wallet> ApplyFundsAsync(int playerId, Currency currency, FundsOperation operation, decimal amount, CancellationToken cancellationToken = default)
    {
        var strategy = _fundsStrategies[operation];
        var wallet = await GetTrackedWalletAsync(playerId, currency, cancellationToken);

        strategy.Execute(wallet, amount);
        await _walletRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Applied {Strategy} of {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})",
            strategy.GetType().Name, amount, playerId, currency, wallet.Balance);
        Refresh(wallet);
        return wallet;
    }
    public async Task AddWalletToPlayerMenuAsync()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var balance = Prompts.PromptAmount("Initial balance");
        if (balance is null)
            return;

        try
        {
            await CreateWalletAsync(playerId.Value, currency.Value, balance.Value);
            Console.WriteLine("Wallet added successfully.");
        }
        catch (PlayerNotFoundException ex)
        {
            _logger.LogWarning(ex, "Could not add wallet, player {PlayerId} not found", playerId);
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (WalletException ex)
        {
            _logger.LogWarning(ex, "Could not add wallet for player {PlayerId} in {Currency}", playerId, currency);
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    public async Task GetWalletsOfPlayerMenuAsync()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var wallets = await GetByPlayerAsync(playerId.Value);

        if (wallets.Count == 0)
        {
            Console.WriteLine("No wallets found for this player.");
            return;
        }

        for (var i = 0; i < wallets.Count; i++)
            Console.WriteLine($"Wallet Number {i} {wallets[i]}");
    }
    public Task DepositToWalletMenuAsync() =>
        RunAmountOperationAsync("Amount to deposit",
            (playerId, currency, amount) => DepositAsync(playerId, currency, amount), "Deposit successful.");

    public Task WithdrawFromWalletMenuAsync() =>
        RunAmountOperationAsync("Amount to withdraw",
            (playerId, currency, amount) => WithdrawAsync(playerId, currency, amount), "Withdrawal successful.");

    public Task BlockWalletMenuAsync() =>
        RunWalletOperationAsync((playerId, currency) => SetBlockedAsync(playerId, currency, true), "Wallet blocked.");

    public Task UnblockWalletMenuAsync() =>
        RunWalletOperationAsync((playerId, currency) => SetBlockedAsync(playerId, currency, false), "Wallet unblocked.");
    public Task UpdateWalletBalanceMenuAsync() =>
        RunAmountOperationAsync("New balance",
            (playerId, currency, amount) => UpdateBalanceAsync(playerId, currency, amount), "Balance updated.");
    public async Task ApplyFundsStrategyMenuAsync()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var operation = Prompts.PromptFundsOperation();
        if (operation is null)
            return;

        var amount = Prompts.PromptAmount("Amount");
        if (amount is null)
            return;

        await GuardWalletOperationAsync(async () =>
        {
            await ApplyFundsAsync(playerId.Value, currency.Value, operation.Value, amount.Value);
            Console.WriteLine($"{operation} operation applied.");
        });
    }
    private void Refresh(Wallet wallet)
    {
        _cache.Set(WalletKey(wallet.Id), wallet, Ttl);
        _cache.Remove(AllWalletsKey);
        _cache.Remove(PlayerWalletsKey(wallet.PlayerId));
        _logger.LogInformation("Cache write-through wallet {WalletId}; list caches invalidated", wallet.Id);
    }
    private async Task<Wallet> GetTrackedWalletAsync(int playerId, Currency currency, CancellationToken cancellationToken)
    {
        return await _walletRepository.GetByPlayerAndCurrencyAsync(playerId, currency, cancellationToken)
            ?? throw new WalletNotFoundException(playerId, currency);
    }
    private async Task RunAmountOperationAsync(string amountLabel, Func<int, Currency, decimal, Task> operation, string successMessage)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var amount = Prompts.PromptAmount(amountLabel);
        if (amount is null)
            return;

        await GuardWalletOperationAsync(async () =>
        {
            await operation(playerId.Value, currency.Value, amount.Value);
            Console.WriteLine(successMessage);
        });
    }
    private async Task RunWalletOperationAsync(Func<int, Currency, Task> operation, string successMessage)
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        await GuardWalletOperationAsync(async () =>
        {
            await operation(playerId.Value, currency.Value);
            Console.WriteLine(successMessage);
        });
    }
    private async Task GuardWalletOperationAsync(Func<Task> operation)
    {
        try
        {
            await operation();
        }
        catch (WalletException ex)
        {
            _logger.LogWarning(ex, "Wallet operation failed");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    private async Task<int> GenerateWalletIdAsync(CancellationToken cancellationToken)
    {
        var existingIds = (await _walletRepository.GetAllAsync(cancellationToken))
            .Select(w => w.Id)
            .ToHashSet();

        int id;
        do
        {
            id = Random.Shared.Next(1, int.MaxValue);
        }
        while (existingIds.Contains(id));

        return id;
    }
}