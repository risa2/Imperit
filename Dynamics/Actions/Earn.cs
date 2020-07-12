using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Earn : IAction
    {
        public (IAction? NewThis, IAction[] Side, State.Player) Do(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return (this, new IAction[0], player == active ? player.Earn() : player);
        }
        public byte Priority => 10;
    }
}