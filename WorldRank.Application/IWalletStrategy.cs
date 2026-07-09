using System;
using System.Collections.Generic;
using System.Text;
using WorldRank.Console;

namespace WorldRank.Application
{
    public interface IWalletStrategy    {
        void Execute(Wallet wallet, decimal amount);
    }
}
