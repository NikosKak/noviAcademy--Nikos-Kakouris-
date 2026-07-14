using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Repositories;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly ILogger<InMemoryPlayerRepository> _logger;

    private readonly List<Player> _players = new();

    public InMemoryPlayerRepository(ILogger<InMemoryPlayerRepository> logger)
    {
        _logger = logger;
    }

    public Task AddAsync(Player player, CancellationToken cancellationToken = default)
    {
        _players.Add(player);
        _logger.LogInformation("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
        return Task.CompletedTask;
    }

    public Task<Player?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_players.FirstOrDefault(item => item.Id == id));
    }

    public Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Return a copy so callers cannot mutate the repository's internal list.
        return Task.FromResult<IReadOnlyList<Player>>(_players.ToList());
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var player = _players.FirstOrDefault(item => item.Id == id);

        if (player is null)
        {
            _logger.LogWarning("Delete skipped: player {PlayerId} not found", id);
            return Task.FromResult(false);
        }

        _players.Remove(player);
        _logger.LogInformation("Player {PlayerId} deleted", id);
        return Task.FromResult(true);
    }
}

