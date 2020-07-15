using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics.Actions
{
    public class Mortality : IAction
    {
        public (IAction[], State.Player) Do(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return (new[] { this }, provinces.Any(prov => prov.IsControlledBy(player)) ? player : player.Die());
        }
        public byte Priority => 200;
    }
}