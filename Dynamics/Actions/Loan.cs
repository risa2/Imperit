using System;
using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Loan : IAction
    {
        readonly State.Settings settings;
        public readonly int Debtor;
        public readonly uint Debt, Remaining, Repayment;

        public Loan(int debtor, uint debt, uint remaining, uint repayment, State.Settings set)
        {
            Debtor = debtor;
            Debt = debt;
            Remaining = remaining;
            Repayment = Math.Min(Remaining, repayment);
            settings = set;
        }
        public (IAction? NewThis, IAction[] Side, State.Player) Do(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            if (player == active && player.Id == Debtor)
            {
                if (Repayment > player.Money)
                {
                    var loan = new Loan(Debtor, Debt, Remaining - player.Money, Repayment, settings);
                    return (loan, new IAction[0], player.Pay(player.Money).LoseCredibility((Repayment - player.Money) / 20.0));
                }
                if (Repayment == Remaining)
                {
                    return (null, new IAction[0], player.Pay(Repayment));
                }
                return (new Loan(Debtor, Debt, Remaining - Repayment, Repayment, settings), new IAction[0], player.Pay(Repayment));
            }
            return (this, new IAction[0], player);
        }
        public bool Allows(ICommand another, IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            if (another is Commands.Loan loan && loan.Player == Debtor)
            {
                return loan.Debt + Remaining <= settings.DebtLimit;
            }
            if (another is Commands.Donation donation && donation.Player == Debtor)
            {
                return donation.Amount + Remaining <= players[donation.Player].Money;
            }
            return true;
        }
        public (IAction, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            return another is Commands.Loan loan && loan.Player == Debtor
                ? (new Loan(Debtor, Debt + loan.Debt, Remaining + loan.Debt, Repayment, settings), false)
                : (this, true);
        }
        public byte Priority => 130;
    }
}