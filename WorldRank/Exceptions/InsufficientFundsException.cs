using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank.Exceptions
{
    public class InsufficientFundsException : WalletExceptions
    {
        public InsufficientFundsException(string message) : base(message)
        {
        }
    }
}
