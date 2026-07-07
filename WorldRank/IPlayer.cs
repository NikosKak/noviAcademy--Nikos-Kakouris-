namespace WorldRank
{
    internal interface IPlayer
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public int Score { get; set; }
        IDictionary<Currency, Wallet> Wallets { get; }
    }
}
