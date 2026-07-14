using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Infrastructure.Repositories;

public class InMemoryWalletRepository : IWalletRepository
{
    private readonly ILogger<InMemoryWalletRepository> _logger;

    private readonly List<Wallet> _wallets = new();

    public InMemoryWalletRepository(ILogger<InMemoryWalletRepository> logger)
    {
        _logger = logger;
    }

    public Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        var exists = _wallets.Any(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency);

        if (exists)
        {
            throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
        }

        _wallets.Add(wallet);
        _logger.LogInformation("Wallet {WalletId} created for player {PlayerId} in {Currency} with balance {Balance}",
            wallet.Id, wallet.PlayerId, wallet.Currency, wallet.Balance);
        return Task.CompletedTask;
    }

    public Task<Wallet?> GetByIdAsync(int walletId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_wallets.FirstOrDefault(item => item.Id == walletId));
    }

    public Task<Wallet?> GetByPlayerAndCurrencyAsync(int playerId, Currency currency, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_wallets.SingleOrDefault(item => item.PlayerId == playerId && item.Currency == currency));
    }

    public Task<IReadOnlyList<Wallet>> GetByPlayerAsync(int playerId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Wallet>>(_wallets.Where(item => item.PlayerId == playerId).ToList());
    }

    public Task<IReadOnlyList<Wallet>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Wallet>>(_wallets.ToList());
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
