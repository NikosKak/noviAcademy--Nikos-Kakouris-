using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank.Exceptions
{
    public class WrongNameException : WalletExceptions
    {
        public WrongNameException(string message) : base(message)
        {
        }
    }
}
