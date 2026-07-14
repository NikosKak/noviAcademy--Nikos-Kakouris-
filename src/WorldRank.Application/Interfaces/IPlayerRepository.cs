using WorldRank.Domain.Entities;

namespace WorldRank.Application.Interfaces;

public interface IPlayerRepository
{
    Task AddAsync(Player player, CancellationToken cancellationToken = default);

    Task<Player?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken cancellationToken = default);

    // Returns true if a player was deleted, false if no such player existed.
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
