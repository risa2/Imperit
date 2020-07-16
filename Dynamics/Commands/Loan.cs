using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Loan : ICommand
    {
        readonly State.Settings settings;
        public readonly int Player;
        public readonly uint Amount, Debt, Repayment;
        public Loan(int player, uint amount, uint debt, uint repayment, State.Settings set)
        {
            Player = player;
            Amount = amount;
            Debt = debt;
            Repayment = repayment;
            settings = set;
        }
        public bool Allowed(IReadOnlyList<State.Player> players, State.Provinces provinces)
            => Debt <= settings.DebtLimit && Amount > 0 && Repayment > 0;
        public (IAction[], State.Player) Perform(State.Player player, State.Provinces provinces)
        {
            return player.Id == Player ? (new[] { new Actions.Loan(Player, Debt, Debt, Repayment, settings) }, player.GainMoney(Amount)) : (System.Array.Empty<IAction>(), player);
        }
    }
}