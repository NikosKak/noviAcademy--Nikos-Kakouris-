using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Services;

public class PlayerService
{
    private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

    private readonly IPlayerRepository _playerRepository;
    private readonly ICache _cache;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(IPlayerRepository playerRepository, ICache cache, ILogger<PlayerService> logger)
    {
        _playerRepository = playerRepository;
        _cache = cache;
        _logger = logger;
    }
    private static string PlayerKey(int id) => $"player:{id}";
    private const string AllPlayersKey = "players:all";
    public async Task<Player> CreateAsync(string name, int score, CancellationToken cancellationToken = default)
    {
        var player = new Player(await GeneratePlayerIdAsync(cancellationToken), name);
        player.AddScore(score);

        await _playerRepository.AddAsync(player, cancellationToken);
        _logger.LogInformation("Player created {PlayerId} {Name} (score {Score})", player.Id, name, score);

        _cache.Set(PlayerKey(player.Id), player, Ttl);
        _cache.Remove(AllPlayersKey);
        _logger.LogInformation("Cache write-through player {PlayerId}; list cache invalidated", player.Id);
        return player;
    }
    public async Task<Player?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGet(PlayerKey(id), out Player? cached) && cached is not null)
        {
            _logger.LogInformation("Cache HIT  player {PlayerId}", id);
            return cached;
        }
        _logger.LogInformation("Cache MISS player {PlayerId} — loading from database", id);
        var player = await _playerRepository.GetByIdAsync(id, cancellationToken);
        if (player is not null)
            _cache.Set(PlayerKey(id), player, Ttl);
        return player;
    }
    public async Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGet(AllPlayersKey, out IReadOnlyList<Player>? cached) && cached is not null)
        {
            _logger.LogInformation("Cache HIT  all players");
            return cached;
        }
        _logger.LogInformation("Cache MISS all players — loading from database");
        var players = await _playerRepository.GetAllAsync(cancellationToken);
        _cache.Set(AllPlayersKey, players, Ttl);
        return players;
    }
    public async Task<Player?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var players = await GetAllAsync(cancellationToken);
        return players.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _playerRepository.DeleteAsync(id, cancellationToken);
        if (deleted)
        {
            _cache.Remove(PlayerKey(id));
            _cache.Remove(AllPlayersKey);
            _logger.LogInformation("Player {PlayerId} deleted; cache invalidated", id);
        }
        return deleted;
    }
    public async Task AddPlayerMenuAsync()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        Console.Write("Score: ");
        if (!int.TryParse(Console.ReadLine(), out var score) || score < 0)
        {
            Console.WriteLine("Score must be a non-negative whole number.");
            return;
        }

        await CreateAsync(name, score);
        Console.WriteLine("Player added successfully.");
    }

    public async Task ListPlayersMenuAsync()
    {
        var all = await GetAllAsync();

        if (all.Count == 0)
        {
            Console.WriteLine("No players registered.");
            return;
        }

        foreach (var player in all)
            Console.WriteLine(player);
    }

    public async Task ListPlayersByScoreMenuAsync()
    {
        var groups = (await GetAllAsync())
            .GroupBy(player => player.Score)
            .OrderByDescending(group => group.Key)
            .ToList();

        if (groups.Count == 0)
        {
            Console.WriteLine("No players registered.");
            return;
        }

        foreach (var group in groups)
        {
            Console.WriteLine($"Score {group.Key}:");
            foreach (var player in group)
                Console.WriteLine($"  {player}");
        }
    }

    public async Task FindPlayerByNameMenuAsync()
    {
        Console.Write("Search by name: ");
        var term = Console.ReadLine() ?? string.Empty;

        var player = await GetByNameAsync(term);
        Console.WriteLine(player is null ? "No player found." : player.ToString());
    }

    public async Task FindPlayerByIdMenuAsync()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var player = await GetByIdAsync(playerId.Value);
        Console.WriteLine(player is null ? "No player found." : player.ToString());
    }

    public async Task DeletePlayerMenuAsync()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var deleted = await DeleteAsync(playerId.Value);
        Console.WriteLine(deleted ? "Player deleted." : "No player found.");
    }
    private async Task<int> GeneratePlayerIdAsync(CancellationToken cancellationToken)
    {
        var existingIds = (await _playerRepository.GetAllAsync(cancellationToken))
            .Select(p => p.Id)
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