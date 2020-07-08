using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Attack : Move
    {
        readonly IReadOnlyList<State.Player> players;
        public Attack(int province, State.IArmy army, IReadOnlyList<State.Player> pls) : base(province, army) => players = pls;
        public override IAction Do(IArray<State.Player> players, State.Provinces provinces, int active)
        {
            (var attacked, var action) = provinces[Province].AttackedBy(Army);
            provinces[Province] = attacked;
            return action.Do(players, provinces, active);
        }
        public override (IAction, bool) Interact(ICommand another)
        {
            if (another is Commands.Attack attack && Army.IsControlledBy(players[attack.Player]) && attack.To == Province)
            {
                return (new Attack(Province, Army.Join(attack.Army), players), false);
            }
            if (another is Commands.Purchase purchase && Army.IsControlledBy(players[purchase.Player]) && purchase.Land == Province)
            {
                return (new Reinforcement(Province, Army, players), true);
            }
            return (this, true);
        }
    }
}