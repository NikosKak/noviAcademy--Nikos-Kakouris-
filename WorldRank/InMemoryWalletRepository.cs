using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        private List<Player> _players;

        public InMemoryWalletRepository(List<Player> players)
        {
            _players = players;
        }
        public void Add(Wallet wallet, int playerId)
        {
            
        }

        public List<Wallet> GetByPlayer(int playerId)
        {
            var wallets = _players.Where(item => item.Id == playerId).SelectMany(item => item.Wallets.Values);
            return wallets.ToList();
        }
    }
}
