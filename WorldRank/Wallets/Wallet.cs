using WorldRank.Currencies;
using WorldRank.Exceptions;
namespace WorldRank.Wallets
{
    public class Wallet
    {
        private decimal _balance;
        public decimal Balance//orizoume oti to balance prepei na einai >=0, alliws tha petaksei exception
        {
            get => _balance;
            private set
            {
                if (value < 0)
                    throw new InsufficientFundsException("Input must not be negative");

                _balance = value;
            }
        }
        //orizoyme tis metavlites toy wallet
        public bool IsBlocked { get; private set; }
        public Currency Currency { get; }
        public int PlayerId { get; private set; }
        public Wallet() { }
        public Wallet(decimal balance, Currency currency, bool isBlocked)
        {
            Balance = balance;
            Currency = currency;
            IsBlocked = isBlocked;
        } //arxikopoioume to wallet me to currency pou theloume na exei kai tis metavlites tou
        public override string ToString() =>
            $"Wallet: {Currency} - Balance: {Balance} ";
    }
}
