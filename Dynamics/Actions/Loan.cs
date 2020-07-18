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
        public (IAction[], State.Player) Perform(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            if (player == active && player.Id == Debtor)
            {
                if (Debt > settings.DebtLimit)
                {
                    return (new[] { new Seizure(player.Id, Debt - player.Money) }, player.Pay(player.Money));
                }
                return (new[] { new Loan(Debtor, (uint)Math.Ceiling(Debt * (1 + settings.Interest)), settings) }, player);
            }
            return (new[] { this }, player);
        }
        public (IAction?, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            return another switch
            {
                Commands.Loan Loan when Loan.Player == Debtor => (new Loan(Debtor, Debt + Loan.Amount, settings), false),
                Commands.Repayment Rep when Rep.Debtor == Debtor => (Rep.Amount >= Debt ? null : new Loan(Debtor, Debt - Rep.Amount, settings), false),
                _ => (this, true)
            };
        }
        public byte Priority => 130;
    }
}