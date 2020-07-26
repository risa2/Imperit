using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics.Actions
{
    public class Mortality : IAction
    {
        public (IAction[], State.Player) Perform(State.Player player, State.Player active, State.IProvinces provinces)
        {
            return (new[] { this }, provinces.Any(prov => prov.IsControlledBy(player.Id)) ? player : player.Die());
        }
        public (IAction[], State.Province) Perform(State.Province province, State.Player active)
        {
            if (province is State.Sea Sea && Sea.Occupied && Sea.Soldiers == 0)
            {
                return (new[] { this }, Sea.Revolt().Item1);
            }
            return (new[] { this }, province);
        }
        public byte Priority => 200;
    }
}