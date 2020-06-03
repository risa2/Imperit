using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Reinforcement : Move
    {
        public Reinforcement(int province, State.IArmy army) : base(province, army) { }
        public override IAction Do(IList<State.Player> players, State.Provinces provinces, int active)
        {
            provinces[Province] = provinces[Province].ReinforcedBy(Army);
            return new Nothing();
        }
        public override (IAction, ICommand?) Interact(ICommand another)
        {
            if (another is Commands.Reinforcement reinf && Army.IsControlledBy(reinf.Player) && reinf.To.Id == Province)
            {
                return (new Actions.Reinforcement(Province, Army.Join(reinf.Army)), null);
            }
            return (this, another);
        }
    }
}