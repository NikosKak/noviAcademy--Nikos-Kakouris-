using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;

namespace WorldRank.Api.DTO
{
    public record CreateWalletRequest(int PlayerId, Currency Currency, decimal InitialBalance = 0m);

    public record DepositRequest(decimal Amount);

    // Response DTO — the shape the client receives. Never expose the domain entity directly.
    public record WalletResponse(int Id, int PlayerId, string Currency, decimal Balance, bool IsBlocked)
    {
        public static WalletResponse From(Wallet wallet) =>
            new(wallet.Id, wallet.PlayerId, wallet.Currency.ToString(), wallet.Balance, wallet.IsBlocked);
    }

}
