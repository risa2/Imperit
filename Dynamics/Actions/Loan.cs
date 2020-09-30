using System;
using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
	public class Loan : IAction
	{
		readonly State.Settings settings;
		public readonly int Debtor;
		public readonly uint Debt;
		public Loan(int debtor, uint debt, State.Settings set)
		{
			Debtor = debtor;
			Debt = debt;
			settings = set;
		}
		public (IAction[], State.Player) Perform(State.Player player, State.Player active, State.IProvinces provinces)
		{
			if (player == active && player.Id == Debtor)
			{
				if (Debt > settings.DebtLimit)
				{
					return (Array.Empty<IAction>(), player.Pay(player.Money));
				}
				return (new[] { new Loan(Debtor, (uint)Math.Ceiling(Debt * (1 + settings.Interest)), settings) }, player);
			}
			return (new[] { this }, player);
		}
		public (IAction[], State.Province) Perform(State.Province province, State.Player active)
		{
			if (active.Id == Debtor && province is State.Land land && land.IsControlledBy(Debtor) && Debt > settings.DebtLimit + active.Money)
			{
				var (new_province, actions) = land.Revolt();
				return (land.Price > Debt ? actions : actions.Concat(new Loan(Debtor, Debt - land.Price, settings)), new_province);
			}
			return (new[] { this }, province);
		}
		public (IAction?, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.IProvinces provinces)
		{
			return another switch
			{
				Commands.Borrow Loan when Loan.Player == Debtor => (new Loan(Debtor, Debt + Loan.Amount, settings), false),
				Commands.Repay Rep when Rep.Debtor == Debtor => (Rep.Amount >= Debt ? null : new Loan(Debtor, Debt - Rep.Amount, settings), false),
				_ => (this, true)
			};
		}
		public byte Priority => 130;
	}
}