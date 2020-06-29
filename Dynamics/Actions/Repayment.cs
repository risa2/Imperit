using System;
using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Repayment : IAction
    {
        private readonly State.Settings settings;
        public readonly int Debtor;
        public readonly uint Debt, Remaining;
        public Repayment(State.Settings set, int debtor, uint debt, uint remaining)
        {
            settings = set;
            Debtor = debtor;
            Debt = debt;
            Remaining = remaining;
        }
        public uint RepaymentAmount => Math.Min((Debt + 4) / 5, Remaining);
        public IAction Do(IList<State.Player> players, State.Provinces provinces, int active)
        {
            if (active == Debtor)
            {
                uint should_pay = RepaymentAmount;
                uint can_pay = Math.Min(should_pay, players[Debtor].Money);
                players[Debtor] = players[Debtor].Pay(can_pay);
                if (should_pay > can_pay)
                {
                    players[Debtor] = players[Debtor].LoseCredibility((should_pay - can_pay) / 20.0);
                    return new Repayment(settings, Debtor, Debt, Remaining - can_pay);
                }
                return Remaining == can_pay ? (IAction)new Nothing() : new Repayment(settings, Debtor, Debt, Remaining - can_pay);
            }
            return this;
        }
        public bool IsOkWith(ICommand another)
        {
            if (another is Commands.Loan loan && loan.Player.Id == Debtor)
            {
                return loan.Debt + Remaining <= settings.DebtLimit;
            }
            if (another is Commands.Donation donation && donation.Player.Id == Debtor)
            {
                return donation.Amount + Remaining <= donation.Player.Money;
            }
            return true;
        }
        public (IAction, bool) Interact(ICommand another)
        {
            if (another is Commands.Loan loan && loan.Player.Id == Debtor)
            {
                return (new Repayment(settings, Debtor, Debt + loan.Debt, Remaining + loan.Debt), false);
            }
            return (this, true);
        }
        public int Priority => 200;
    }
}