using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics.Commands
{
    public class Purchase : ICommand
    {
        public readonly int Player, Land;
        public Purchase(int player, int land)
        {
            Player = player;
            Land = land;
        }
        public bool Allowed(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces)
            => players[Player].Money >= (provinces[Land] as State.Land)!.Price && provinces.NeighborsOf(provinces[Land]).Any(prov => prov is State.Land land && land.IsControlledBy(players[Player]));
        public IAction Do(State.Settings settings, IArray<State.Player> players, State.Provinces provinces)
        {
            IAction action;
            players[Player] = players[Player].Pay((provinces[Land] as State.Land)!.Price);
            (provinces[Land], action) = provinces[Land].GiveUpTo(new State.PlayerArmy(settings, players[Player], 0));
            return action;
        }
    }
}