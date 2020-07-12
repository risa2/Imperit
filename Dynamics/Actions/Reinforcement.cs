using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Reinforcement : Move
    {
        public Reinforcement(int province, State.IArmy army) : base(province, army) { }
        public override (IAction? NewThis, IAction[] Side, State.Province) Do(State.Province province, State.Player active)
        {
            return Province == province.Id ? (null, new IAction[0], province.ReinforcedBy(Army)) : (this, new IAction[0], province);
        }
        public override (IAction, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            if (another is Commands.Reinforcement reinf && Army.IsControlledBy(players[reinf.Player]) && reinf.To.Id == Province)
            {
                return (new Reinforcement(Province, Army.Join(reinf.Army)), false);
            }
            return (this, true);
        }
    }
}