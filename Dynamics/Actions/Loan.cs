using Imperit.State;
using System;

namespace Imperit.Dynamics.Actions
{
	public class Loan : IAction
	{
		readonly Settings settings;
		public readonly int Debtor;
		public readonly int Debt;
		public Loan(int debtor, int debt, Settings set)
		{
			Debtor = debtor;
			Debt = debt;
			settings = set;
		}
		public (IAction?, Player) Perform(Player player, Player active, IProvinces provinces)
		{
			if (player == active && player.Id == Debtor)
			{
				int next_debt = Debt + (int)Math.Ceiling(Debt * settings.Interest);
				if (next_debt <= player.Money)
				{
					return (null, player.ChangeMoney(-next_debt));
				}
				return (new Loan(Debtor, next_debt - player.Money, settings), player.ChangeMoney(-player.Money));
			}
			return (this, player);
		}
		public (IAction?, Province) Perform(Province province, Player active)
		{
			if (active.Id == Debtor && province is Land land && land.IsAllyOf(Debtor) && Debt > settings.DebtLimit + active.Money)
			{
				return (land.Price > Debt ? null : new Loan(Debtor, Debt - land.Price, settings), land.Revolt());
			}
			return (this, province);
		}
		public (IAction?, bool) Interact(ICommand another) => another switch
		{
			Commands.Borrow Loan when Loan.Player == Debtor => (new Loan(Debtor, Debt + Loan.Amount, settings), false),
			_ => (this, true)
		};
		public byte Priority => 130;
	}
}