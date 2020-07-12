using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics.Actions
{
    public class Mortality : IAction
    {
        public (IAction? NewThis, IAction[] Side, State.Player) Do(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return (this, new IAction[0], provinces.Any(prov => prov.IsControlledBy(player)) ? player : player.Die());
        }
        public byte Priority => 200;
    }
}