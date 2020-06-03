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
        public IAction Do(IList<State.Player> players, State.Provinces provinces, int active)
        {
            players[Player] = players[Player].DecreaseIncome(Change);
            return new Nothing();
        }
    }
}