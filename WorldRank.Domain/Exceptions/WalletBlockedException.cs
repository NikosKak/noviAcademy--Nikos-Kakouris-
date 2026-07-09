using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Console.Exceptions
{
	public class WalletBlockedException : WalletException
	{
		public Currency Currency { get; }

		public WalletBlockedException(Currency currency)
			: base($"The {currency} wallet is blocked. Unblock it before performing this operation.")
		{
			Currency = currency;
		}
	}
}
