namespace WorldRank
{
    public class Player :IPlayer
    {
        //orizoyme metavlites toy Player
        public int Id { get; set; }
        public string Name { get;set; }
        public int Score { get; set; }
        //orizoume to wallet tou player
        public Dictionary<Currency, Wallet> Wallets { get; set; } = new Dictionary<Currency, Wallet>();
        //arxikopoioume ton player
        public Player(string name, int id) {
            Id = id;
            Name = name;
        }
        public void UpdateScore(int newscore)
        {
            Score = newscore;
        }
        public override string ToString() =>
            $"[{Id}] {Name} - Score: {Score}";

    }
}
