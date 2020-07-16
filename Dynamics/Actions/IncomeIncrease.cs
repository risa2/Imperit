using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class IncomeIncrease : IAction
    {
        public readonly int Player;
        public readonly uint Change;
        public IncomeIncrease(int player, uint change)
        {
            Player = player;
            Change = change;
        }
        public (IAction[], State.Player) Perform(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return player.Id == Player ? (System.Array.Empty<IAction>(), player.IncreaseIncome(Change)) : (new[] { this }, player);
        }
        public byte Priority => 40;
    }
}