using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Earn : IAction
    {
        public readonly int Player;
        public Earn(int player) => Player = player;
        public IAction Do(IList<State.Player> players, State.Provinces provinces, int active)
        {
            if (active == Player)
                players[Player] = players[Player].Earn();
            return this;
        }
        public int Priority => 50;
    }
}