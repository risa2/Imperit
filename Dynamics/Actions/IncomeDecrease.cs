using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class IncomeDecrease : IAction
    {
        public readonly int Player;
        public readonly uint Change;
        public IncomeDecrease(int player, uint change)
        {
            Player = player;
            Change = change;
        }
        public (IAction[], State.Player) Do(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return player.Id == Player ? (System.Array.Empty<IAction>(), player.DecreaseIncome(Change)) : (new[] { this }, player);
        }
        public byte Priority => 40;
    }
}