using System;
using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Loan : IAction
    {
        readonly State.Settings settings;
        readonly IReadOnlyList<State.Player> players;
        public readonly int Debtor;
        public readonly uint Debt, Remaining, Repayment;
        public Loan(State.Settings set, IReadOnlyList<State.Player> pls, int debtor, uint debt, uint remaining, uint repayment)
        {
            settings = set;
            players = pls;
            Debtor = debtor;
            Debt = debt;
            Remaining = remaining;
            Repayment = Math.Min(Remaining, repayment);
        }
        public IAction Do(IArray<State.Player> players, State.Provinces provinces, int active)
        {
            if (active == Debtor)
            {
                uint can_pay = Math.Min(Repayment, players[Debtor].Money);
                players[Debtor] = players[Debtor].Pay(can_pay);
                if (Repayment > can_pay)
                {
                    players[Debtor] = players[Debtor].LoseCredibility((Repayment - can_pay) / 20.0);
                    return new Loan(settings, this.players, Debtor, Debt, Remaining - can_pay, Repayment);
                }
                return Remaining == can_pay ? (IAction)new Nothing() : new Loan(settings, this.players, Debtor, Debt, Remaining - can_pay, Repayment);
            }
            return this;
        }
        public bool Allows(ICommand another)
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
        public (IAction, bool) Interact(ICommand another)
        {
            return another is Commands.Loan loan && loan.Player == Debtor
                ? (new Loan(settings, players, Debtor, Debt + loan.Debt, Remaining + loan.Debt, Repayment), false)
                : (this, true);
        }
        public int Priority => 200;
    }
}