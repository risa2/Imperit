using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Loan : ICommand
    {
        public readonly int Player;
        public readonly uint Amount, Debt, Repayment;
        public Loan(int player, uint amount, uint debt, uint repayment)
        {
            Player = player;
            Amount = amount;
            Debt = debt;
            Repayment = repayment;
        }
        public bool Allowed(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces)
            => Debt <= settings.DebtLimit && Amount > 0 && Repayment > 0;
        public IAction Do(State.Settings settings, IArray<State.Player> players, State.Provinces provinces)
        {
            players[Player] = players[Player].GainMoney(Amount);
            return new Actions.Loan(settings, players, Player, Debt, Debt, Repayment);
        }
    }
}