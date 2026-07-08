using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WorldRank.Exceptions
{
    public class WalletExceptions : Exception
    {
        public WalletExceptions()
        {
        }

        public WalletExceptions(string message)
            : base(message)
        {
        }

        public WalletExceptions(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
