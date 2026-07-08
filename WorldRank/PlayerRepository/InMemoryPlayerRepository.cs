using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Players;

namespace WorldRank.PlayerRepository
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        //orizoume to private List<Player> _players; pou tha periexei tous paiktes pou tha dimiourgisoume
        private List<Player> _players;
        //constructor pou dexetai mia lista me paiktes kai tin apothikeuei sto private List<Player> _players;
        public InMemoryPlayerRepository(List<Player> players)
        {
            _players = players;
        }
        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public void DeletePlayer(int playerid)
        {
            var player = _players.Where(item => item.Id == playerid).FirstOrDefault();

            if (player != null)
            {
                _players.Remove(player);
            }
        }

        public Player FindPlayer(int playerid)
        {
            return _players.Where(item => item.Id == playerid).FirstOrDefault();
        }

        IEnumerable<IGrouping<int, Player>> IPlayerRepository.GroupPlayersByScore()
        {
            throw new NotImplementedException();
        }
    }
}
