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
        public IAction Do(IArray<State.Player> players, State.Provinces provinces, int active)
        {
            players[Player] = players[Player].IncreaseIncome(Change);
            return new Nothing();
        }
    }
}