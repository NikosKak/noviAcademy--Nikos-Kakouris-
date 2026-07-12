using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Console;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Application
{
    public class SubtractFunds : IWalletStrategy
    {
        public void Execute(Wallet wallet, decimal amount)
        {
            wallet.Withdraw(amount);
        }
    }
}
