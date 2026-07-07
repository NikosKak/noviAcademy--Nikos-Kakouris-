namespace WorldRank
{
    public interface IPlayerRepository
    {
        void Addplayer(Player p);
        Player FindPlayer(int id);
        void RemovePlayer(int id);
    }
}
