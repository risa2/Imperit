using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics.Commands
{
    public class Purchase : ICommand
    {
        public State.Player Player { get; }
        private readonly State.Settings settings;
        private readonly State.Provinces provinces;
        public readonly State.Land Land;
        public Purchase(State.Settings settings, State.Player player, State.Land land, State.Provinces provinces)
        {
            Player = player;
            this.settings = settings;
            this.provinces = provinces;
            Land = land;
        }
        public bool Allowed => Player.Money >= Land.Price && provinces.NeighborsOf(Land).Any(prov => prov is State.Land land && land.IsControlledBy(Player));
        public IAction Consequences { get; private set; } = new Actions.Nothing();
        public void Do(IList<State.Player> players, State.Provinces provinces)
        {
            players[Player.Id] = Player.Pay(Land.Price);
            (provinces[Land.Id], Consequences) = Land.GiveUpTo(new State.PlayerArmy(settings, Player, 0));
        }
    }
}