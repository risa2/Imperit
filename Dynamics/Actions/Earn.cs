using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Earn : IAction
    {
        public (IAction[], State.Player) Perform(State.Player player, State.Player active, State.IProvinces provinces)
        {
            return (new[] { this }, player == active ? player.Earn() : player);
        }
        public byte Priority => 10;
    }
}