using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Console;

namespace WorldRank.Application
{
    public class ForceSubtractFunds : IWalletStrategy
    {
        public void Execute(Wallet wallet, decimal amount)
        {
            wallet.ForcedWithdraw(amount);
        }
    }
}
