using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class IncomeChange : IAction
    {
        public readonly int Player, Change;
        public IncomeChange(int player, int change)
        {
            Player = player;
            Change = change;
        }
        public (IAction[], State.Player) Perform(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return player.Id == Player ? (System.Array.Empty<IAction>(), player.ChangeIncome(Change)) : (new[] { this }, player);
        }
        public byte Priority => 40;
    }
}