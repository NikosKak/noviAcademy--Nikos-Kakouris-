namespace WorldRank
{
    internal interface IPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        //kolame to wallet ston player gia na exoume prosbasi se auto
        Dictionary<Currency, Wallet> Wallets { get; set; }
    }
}
