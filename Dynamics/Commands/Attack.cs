using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Attack : Move
    {
        public Attack(int player, int from, int to, State.IArmy army) : base(player, from, to, army) { }
        public override IAction Do(State.Settings settings, IArray<State.Player> players, State.Provinces provinces)
        {
            provinces[From] = provinces[From].StartMove(provinces[To], Army);
            return new Actions.Attack(To, Army, players);
        }
    }
}