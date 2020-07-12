using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics.Commands
{
    public class Purchase : ICommand
    {
        public readonly State.PlayerArmy Army;
        public readonly int Land;
        public readonly uint Price;
        public Purchase(State.PlayerArmy army, int land, uint price)
        {
            Army = army;
            Land = land;
            Price = price;
        }
        public bool Allowed(IReadOnlyList<State.Player> players, State.Provinces provinces)
            => players[Army.Player.Id].Money >= Price && provinces.NeighborsOf(provinces[Land]).Any(prov => prov is State.Land land && land.IsAllyOf(Army));

        public (IAction[], State.Province) Do(State.Province province)
        {
            return province.Id == Land ? province.GiveUpTo(Army).Swap() : (new IAction[0], province);
        }
        public (IAction[], State.Player) Do(State.Player player, State.Provinces provinces)
        {
            return (new IAction[0], player == Army.Player ? player.Pay(Price) : player);
        }
    }
}