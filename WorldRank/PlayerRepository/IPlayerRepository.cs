using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Players;

namespace WorldRank.PlayerRepository
{
    public interface IPlayerRepository
    {
        void AddPlayer(Player player);
        Player FindPlayer(int playerid);
        void DeletePlayer(int playerid);
        IEnumerable<IGrouping<int, Player>> GroupPlayersByScore();
    }
}
