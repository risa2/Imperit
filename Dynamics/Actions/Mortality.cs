using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics.Actions
{
    public class Mortality : IAction
    {
        public (IAction[], State.Player) Perform(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return (new[] { this }, provinces.Any(prov => prov.IsControlledBy(player.Id)) ? player : player.Die());
        }
        public byte Priority => 200;
    }
}