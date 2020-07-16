using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Reinforcement : Move
    {
        public Reinforcement(int province, State.IArmy army) : base(province, army) { }
        public override (IAction[], State.Province) Perform(State.Province province, State.Player active)
        {
            return Province == province.Id ? (System.Array.Empty<IAction>(), province.ReinforcedBy(Army)) : (new[] { this }, province);
        }
        public override (IAction, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            if (another is Commands.Reinforcement reinf && Army.IsControlledBy(reinf.Player) && reinf.To.Id == Province)
            {
                return (new Reinforcement(Province, Army.Join(reinf.Army)), false);
            }
            if (another is Commands.Recruitment recr && Army.IsControlledBy(recr.Player) && recr.Land == Province)
            {
                return (new Reinforcement(Province, Army.Join(recr.Army)), false);
            }
            return (this, true);
        }
    }
}