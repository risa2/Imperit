using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Battle : ArmyAction
    {
        public Battle(int province, State.Army army) : base(province, army) { }
        public override (IAction[], State.Province) Perform(State.Province province, State.Player active)
        {
            if (Province == province.Id)
            {
                var (attacked, actions) = province.AttackedBy(Army);
                return (actions, attacked);
            }
            return (new[] { this }, province);
        }
        public override (IAction, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.IProvinces provinces)
        {
            if (another is Commands.Attack attack && Army.IsControlledBy(attack.Player) && attack.To.Id == Province)
            {
                return (new Battle(Province, Army.Join(attack.Army)), false);
            }
            if (another is Commands.Purchase purchase && Army.IsControlledBy(purchase.Player.Id) && purchase.Land == Province)
            {
                return (new Reinforcement(Province, Army), true);
            }
            return (this, true);
        }
    }
}