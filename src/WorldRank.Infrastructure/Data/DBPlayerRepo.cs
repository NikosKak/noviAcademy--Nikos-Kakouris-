using Microsoft.EntityFrameworkCore;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Data
{
    internal class DBPlayerRepo : IPlayerRepository
    {
        private readonly WorldRankDbContext _context;

        public DBPlayerRepo(WorldRankDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Player player, CancellationToken cancellationToken = default)
        {
            await _context.Players.AddAsync(player, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<Player?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _context.Players.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        // Read-only listing — AsNoTracking avoids change-tracker overhead.
        public async Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Players.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (player is null)
                return false;

            _context.Players.Remove(player);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
