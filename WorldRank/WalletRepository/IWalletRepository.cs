using WorldRank.Wallets;
using WorldRank.Players;
namespace WorldRank.WalletRepository
{
    public interface IWalletRepository
    {
        void AddWallet(Wallet wallet,int playerId);
        List<Wallet> GetPlayer(int playerId);
    }
}
