using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Instability : IAction
    {
        static readonly System.Random rand = new System.Random();
        public readonly int Land;
        public readonly int? LoyalTo;
        public Instability(int land, int? loyalTo)
        {
            Land = land;
            LoyalTo = loyalTo;
        }
        public IAction Do(IList<State.Player> players, State.Provinces provinces, int active)
        {
            if (provinces[Land].IsControlledBy(players[active]) && active != LoyalTo && rand.NextDouble() < (provinces[Land] as State.Land)!.Instability)
            {
                (var revolted, var action) = provinces[Land].Revolt();
                provinces[Land] = revolted;
                return new Combination(this, action.Do(players, provinces, active));
            }
            return this;
        }
        public int Priority => 700;
    }
}