using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Console;

namespace WorldRank.Application
{
    public class AddFundsWallet : IWalletStrategy
    {
        void IWalletStrategy.Execute(Wallet wallet, decimal amount)
        {
            wallet.Withdraw(amount);
        }
    }
}
