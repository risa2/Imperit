using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Attack : Move
    {
        public Attack(int province, State.IArmy army) : base(province, army) { }
        public override IAction Do(IList<State.Player> players, State.Provinces provinces, int active)
        {
            (var attacked, var action) = provinces[Province].AttackedBy(Army);
            provinces[Province] = attacked;
            return action.Do(players, provinces, active);
        }
        public override (IAction, bool) Interact(ICommand another)
        {
            if (another is Commands.Attack attack && Army.IsControlledBy(attack.Player) && attack.To.Id == Province)
            {
                return (new Attack(Province, Army.Join(attack.Army)), false);
            }
            if (another is Commands.Purchase purchase && Army.IsControlledBy(purchase.Player) && purchase.Land.Id == Province)
            {
                return (new Reinforcement(Province, Army), true);
            }
            return (this, true);
        }
    }
}