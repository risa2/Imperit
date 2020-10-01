using System.Linq;
using System.Collections.Generic;
using Imperit.State;

namespace Imperit.Dynamics.Commands
{
    public class Purchase : ICommand
    {
        public readonly Player Player;
        public readonly int Land;
        public readonly int Price;
        public Purchase(Player player, int land, int price)
        {
            Player = player;
            Land = land;
            Price = price;
        }
        public bool Allowed(IReadOnlyList<Player> players, IProvinces provinces)
            => players[Player.Id].Money >= Price && provinces.NeighborsOf(Land).Any(prov => prov is Land land && land.IsControlledBy(Player.Id));

        public (IAction[], Province) Perform(Province province)
        {
            return province.Id == Land ? province.GiveUpTo(Player).Swap() : (System.Array.Empty<IAction>(), province);
        }
        public (IAction[], Player) Perform(Player player, IProvinces provinces)
        {
            return (System.Array.Empty<IAction>(), Player == player ? player.Pay(Price) : player);
        }
    }
}