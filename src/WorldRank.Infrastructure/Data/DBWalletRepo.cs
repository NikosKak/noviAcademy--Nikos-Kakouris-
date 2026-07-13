using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;

namespace WorldRank.Infrastructure.Data
{
    internal class DBWalletRepo : IWalletRepository
    {
        private readonly WorldRankDbContext _context;

        public DBWalletRepo(WorldRankDbContext context)
        {
            _context = context;
        }
        public void Add(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
        }

        public void Block(int playerId, Currency currency)
        {
            _context.Wallets.Find(playerId, currency)?.Block();
            _context.SaveChanges();
        }

        public void Deposit(int playerId, Currency currency, decimal amount)
        {
            _context.Wallets.Find(playerId, currency)?.Deposit(amount);
            _context.SaveChanges();
        }

        public Wallet[] GetAll()
        {
            return _context.Wallets.ToArray();
        }

        public List<Wallet> GetAllWalletsByPlayerId(int playerId)
        {
            return _context.Wallets.Where(w => w.PlayerId == playerId).ToList();
        }

        public Wallet GetWallet(int playerId, Currency currency)
        {
            var wallet = _context.Wallets
                .FirstOrDefault(w => w.PlayerId == playerId && w.Currency == currency);
            if (wallet == null)
                throw new InvalidOperationException("Wallet not found.");
            return wallet;
        }

        public void Unblock(int playerId, Currency currency)
        {
            _context.Wallets.Find(playerId, currency)?.Unblock();
            _context.SaveChanges();
        }

        public void UpdateBalance(int playerId, Currency currency, decimal newBalance)
        {
            var wallet = _context.Wallets.Find(playerId, currency);
            if (wallet == null)
                throw new InvalidOperationException("Wallet not found.");
            wallet.SetBalance(newBalance);
            _context.SaveChanges();
        }

        public void Withdraw(int playerId, Currency currency, decimal amount)
        {
            var wallet = _context.Wallets.Find(playerId, currency);
            if (wallet == null)
                throw new InvalidOperationException("Wallet not found.");
            wallet.Withdraw(amount);
            _context.SaveChanges();
        }
    }
}
