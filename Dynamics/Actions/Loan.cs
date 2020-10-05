using System;

namespace Imperit.Dynamics.Actions
{
	public class Loan : IAction
	{
		readonly State.Settings settings;
		public readonly int Debtor;
		public readonly int Debt;
		public Loan(int debtor, int debt, State.Settings set)
		{
			Debtor = debtor;
			Debt = debt;
			settings = set;
		}
		public (IAction[], State.Player) Perform(State.Player player, State.Player active, State.IProvinces provinces)
		{
			if (player == active && player.Id == Debtor)
			{
				int next_debt = Debt + (int)Math.Ceiling(Debt * settings.Interest);
				if (next_debt <= player.Money)
				{
					return (Array.Empty<IAction>(), player.ChangeMoney(-next_debt));
				}
				return (new[] { new Loan(Debtor, next_debt - player.Money, settings) }, player.ChangeMoney(-player.Money));
			}
			return (new[] { this }, player);
		}
		public (IAction[], State.Province) Perform(State.Province province, State.Player active)
		{
			if (active.Id == Debtor && province is State.Land land && land.IsAllyOf(Debtor) && Debt > settings.DebtLimit + active.Money)
			{
				return (land.Price > Debt ? Array.Empty<IAction>() : new[] { new Loan(Debtor, Debt - land.Price, settings) }, land.Revolt());
			}
			return (new[] { this }, province);
		}
		public (IAction?, bool) Interact(ICommand another) => another switch
		{
			Commands.Borrow Loan when Loan.Player == Debtor => (new Loan(Debtor, Debt + Loan.Amount, settings), false),
			_ => (this, true)
		};
		public byte Priority => 130;
	}
}