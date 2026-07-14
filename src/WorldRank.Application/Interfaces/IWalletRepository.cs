using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;

namespace WorldRank.Application.Interfaces;

public interface IWalletRepository
{
    Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default);
    Task<Wallet?> GetByIdAsync(int walletId, CancellationToken cancellationToken = default);
    Task<Wallet?> GetByPlayerAndCurrencyAsync(int playerId, Currency currency, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Wallet>> GetByPlayerAsync(int playerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Wallet>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
