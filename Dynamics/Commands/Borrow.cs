using Imperit.State;
using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
	public class Borrow : ICommand
	{
		readonly Settings settings;
		public readonly int Player;
		public readonly int Amount;
		public Borrow(int player, int amount, Settings set)
		{
			Player = player;
			Amount = amount;
			settings = set;
		}
		public bool Allowed(IReadOnlyList<Player> players, IProvinces provinces)
		{
			return Amount <= settings.DebtLimit && Amount > 0;
		}
		public (IAction?, Player) Perform(Player player, IProvinces provinces)
		{
			return player.Id == Player ? (new Actions.Loan(Player, Amount, settings), player.ChangeMoney(Amount)) : (null, player);
		}
	}
}