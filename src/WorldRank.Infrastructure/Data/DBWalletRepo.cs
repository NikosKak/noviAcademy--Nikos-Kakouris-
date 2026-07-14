using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Infrastructure.Data
{
    internal class DBWalletRepo : IWalletRepository
    {
        private readonly WorldRankDbContext _context;
        public DBWalletRepo(WorldRankDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default)
        {
            var exists = await _context.Wallets
                .AnyAsync(w => w.PlayerId == wallet.PlayerId && w.Currency == wallet.Currency, cancellationToken);
            if (exists)
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);

            await _context.Wallets.AddAsync(wallet, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public Task<Wallet?> GetByIdAsync(int walletId, CancellationToken cancellationToken = default)
        {
            return _context.Wallets.FirstOrDefaultAsync(w => w.Id == walletId, cancellationToken);
        }
        public Task<Wallet?> GetByPlayerAndCurrencyAsync(int playerId, Currency currency, CancellationToken cancellationToken = default)
        {
            return _context.Wallets
                .FirstOrDefaultAsync(w => w.PlayerId == playerId && w.Currency == currency, cancellationToken);
        }
        public async Task<IReadOnlyList<Wallet>> GetByPlayerAsync(int playerId, CancellationToken cancellationToken = default)
        {
            return await _context.Wallets
                .AsNoTracking()
                .Where(w => w.PlayerId == playerId)
                .ToListAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<Wallet>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Wallets.AsNoTracking().ToListAsync(cancellationToken);
        }
        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
