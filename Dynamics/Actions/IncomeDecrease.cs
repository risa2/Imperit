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
        public (IAction? NewThis, IAction[] Side, State.Player) Do(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return player.Id == Player ? (null, new IAction[0], player.DecreaseIncome(Change)) : (this, new IAction[0], player);
        }
        public byte Priority => 40;
    }
}