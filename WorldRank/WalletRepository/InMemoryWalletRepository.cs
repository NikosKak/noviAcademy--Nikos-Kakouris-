using WorldRank.Players;
using WorldRank.Wallets;

namespace WorldRank.WalletRepository
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        //tora orizoyme tis metavlites poy tha xrisimopoihsoume sthn klasi InMemoryWalletRepository
        //edo orizoume thn lista twn paikton pou tha xrisimopoihsoume gia na apothikeusoume ta wallets
        private List<Player> _players;
        //arxikopoioyme thn klasi InMemoryWalletRepository me thn lista twn paikton pou tha dexetai ws parametro
        public InMemoryWalletRepository(List<Player> players)
        {
            _players = players;
        }
        public void AddWallet(Wallet wallet, int playerId)
        {
            var player = _players.Where(item => item.Id == playerId).SingleOrDefault();

            if (player != null)
            {
                player.Wallets.Add(wallet.Currency, wallet);
            }
        }

        public List<Wallet> GetPlayer(int playerId)
        {
            var wallets = _players.Where(item => item.Id == playerId).SelectMany(item => item.Wallets.Values);
            return wallets.ToList();
        }
    }
}
