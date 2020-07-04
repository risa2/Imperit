using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Reinforcement : Move
    {
        readonly IReadOnlyList<State.Player> players;
        public Reinforcement(int province, State.IArmy army, IReadOnlyList<State.Player> players) : base(province, army) => this.players = players;
        public override IAction Do(IList<State.Player> players, State.Provinces provinces, int active)
        {
            provinces[Province] = provinces[Province].ReinforcedBy(Army);
            return new Nothing();
        }
        public override (IAction, bool) Interact(ICommand another)
        {
            if (another is Commands.Reinforcement reinf && Army.IsControlledBy(players[reinf.Player]) && reinf.To == Province)
            {
                return (new Reinforcement(Province, Army.Join(reinf.Army), players), false);
            }
            return (this, true);
        }
    }
}