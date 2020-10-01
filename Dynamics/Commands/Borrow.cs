using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
	public class Borrow : ICommand
	{
		readonly State.Settings settings;
		public readonly int Player;
		public readonly int Amount;
		public Borrow(int player, int amount, State.Settings set)
		{
			Player = player;
			Amount = amount;
			settings = set;
		}
		public bool Allowed(IReadOnlyList<State.Player> players, State.IProvinces provinces)
		{
			return Amount <= settings.DebtLimit && Amount > 0;
		}
		public (IAction[], State.Player) Perform(State.Player player, State.IProvinces provinces)
		{
			return player.Id == Player ? (new[] { new Actions.Loan(Player, Amount, settings) }, player.GainMoney(Amount)) : (System.Array.Empty<IAction>(), player);
		}
	}
}