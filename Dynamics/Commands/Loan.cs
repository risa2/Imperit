using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Loan : ICommand
    {
        private readonly State.Settings settings;
        public readonly State.Player Player;
        public readonly uint Amount, Debt;
        public Loan(State.Settings settings, State.Player player, uint amount, uint debt)
        {
            this.settings = settings;
            Player = player;
            Amount = amount;
            Debt = debt;
        }
        public bool Allowed => Debt <= settings.DebtLimit && Amount > 0;
        public void Do(IList<State.Player> players, State.Provinces provinces) => players[Player.Id] = Player.GainMoney(Amount);
        public IAction Consequences => new Actions.Repayment(settings, Player.Id, Debt, Debt);
    }
}