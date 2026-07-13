using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Data
{
    internal class DBPlayerRepo : IPlayerRepository
    {
        //private field pou krata reference sto context pou tha xrisimopoihthei gia na kanoume ta operations stin vasi dedomenon
        private readonly WorldRankDbContext _context;
        //Constructor injection pou dexetai to context kai to apothikeuei sto private field
        public DBPlayerRepo(WorldRankDbContext context)
        {
            _context = context;
        }
        public async Task<Player?> GetByIdAsync(int id)
        {
            return await _context.Players.FindAsync(id);
        }
        public async Task AddAsync(Player player)
        {
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
        }
        public void AddPlayer(Player player)
        {
            //prosthetei ton player sto context kai apothikeuei tis allages stin vasi dedomenon
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public void DeletePlayer(int playerId)
        {
            var player = _context.Players.Find(playerId);
            if (player != null) { 
                _context.Players.Remove(player);
                _context.SaveChanges();
            }
        }

        public Player? FindPlayer(int playerId)
        {
            return _context.Players.Find(playerId);
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Players.ToList();
        }

        public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
        {
            return _context.Players.GroupBy(p => p.Score).ToList();
        }
    }
}
