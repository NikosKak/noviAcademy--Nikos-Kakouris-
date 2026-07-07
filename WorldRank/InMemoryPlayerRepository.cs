using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank
{
    internal class InMemoryPlayerRepository : IPlayerRepository
    {
        //orizoume to private List<Player> _players; pou tha periexei tous paiktes pou tha dimiourgisoume
        private List<Player> _players;
        //constructor pou dexetai mia lista me paiktes kai tin apothikeuei sto private List<Player> _players;
        public InMemoryPlayerRepository(List<Player> players)
        {
            _players = players;
        }

        public void Addplayer(Player p)
        {
            _players.Add(p);
        }

        public Player FindPlayer(int id)
        {
            var result = _players.FirstOrDefault(p => p.Id == id);
            return result;
        }

        public void RemovePlayer(int id)
        {
            var player = _players.FirstOrDefault(x => x.Id == id);

            if (player != null)
            {
                _players.Remove(player);
            }
        }
    }
}
