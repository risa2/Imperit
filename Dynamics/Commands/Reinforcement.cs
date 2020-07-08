namespace Imperit.Dynamics.Commands
{
    public class Reinforcement : Move
    {
        public Reinforcement(int player, int from, int to, State.IArmy army) : base(player, from, to, army) { }
        public override IAction Do(State.Settings settings, IArray<State.Player> players, State.Provinces provinces)
        {
            provinces[From] = provinces[From].StartMove(provinces[To], Army);
            return new Actions.Reinforcement(To, Army, players);
        }
    }
}