using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Earn : IAction
    {
        public (IAction[], State.Player) Do(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return (new[] { this }, player == active ? player.Earn() : player);
        }
        public byte Priority => 10;
    }
}