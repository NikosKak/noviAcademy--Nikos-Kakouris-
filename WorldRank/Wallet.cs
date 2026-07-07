using System;
using System.Collections.Generic;
using System.Text;

namespace WorldRank
{
    public enum Currency
    {
        USD,
        EUR,
        NONE
    }
    public class Wallet 
    {
        private decimal _balance;
        public decimal Balance//orizoume oti to balance prepei na einai >=0, alliws tha petaksei exception
        {
            get => _balance;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Balance cannot go negative.");

                _balance = value;
            }
        }
        //orizoyme tis metavlites toy wallet
        public bool IsBlocked { get; private set; }
        Currency Currency { get; }
        public int PlayerId { get; private set; }
        public Wallet() { }
        public Wallet(decimal balance, Currency currency, bool isBlocked)
        {
            Balance = balance;
            Currency = currency;
            IsBlocked = false;
        } //arxikopoioume to wallet me to currency pou theloume na exei kai tis metavlites tou

    }
}
