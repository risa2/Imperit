using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
	public class Repay : ICommand
	{
		public readonly int Debtor;
		public readonly int Amount;
		public Repay(int debtor, int amount)
		{
			Debtor = debtor;
			Amount = amount;
		}
		public (IAction[], State.Player) Perform(State.Player player, State.IProvinces provinces)
		{
			return (System.Array.Empty<IAction>(), player.Id == Debtor ? player.Pay(Amount) : player);
		}
		public bool Allowed(IReadOnlyList<State.Player> players, State.IProvinces provinces) => players[Debtor].Money >= Amount;
	}
}
