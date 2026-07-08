using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank.Exceptions
{
    public class PlayerException : Exception
    {
        public PlayerException()
        {
        }

        public PlayerException(string message)
            : base(message)
        {
        }

        public PlayerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
