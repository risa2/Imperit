using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Loan : ICommand
    {
        readonly State.Settings settings;
        public readonly int Player;
        public readonly uint Amount;
        public Loan(int player, uint amount, State.Settings set)
        {
            Player = player;
            Amount = amount;
            settings = set;
        }
        public bool Allowed(IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            return Amount <= settings.DebtLimit && Amount > 0;
        }
        public (IAction[], State.Player) Perform(State.Player player, State.Provinces provinces)
        {
            return player.Id == Player ? (new[] { new Actions.Loan(Player, Amount, settings) }, player.GainMoney(Amount)) : (System.Array.Empty<IAction>(), player);
        }
    }
}