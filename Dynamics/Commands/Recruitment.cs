using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Recruitment : ICommand
    {
        public State.Player Player { get; }
        private readonly State.Settings settings;
        public readonly State.Land Land;
        public readonly uint Soldiers;
        public Recruitment(State.Settings set, State.Player player, State.Land land, uint soldiers)
        {
            Player = player;
            settings = set;
            Land = land;
            Soldiers = soldiers;
        }
        public IAction Consequences => new Actions.Reinforcement(Land.Id, new State.PlayerArmy(settings, Player, Soldiers));
        public void Do(IList<State.Player> players, State.Provinces provinces) => players[Player.Id] = Player.Pay(Soldiers);
        public bool Allowed => Land.IsControlledBy(Player) && Player.Money >= Soldiers && Soldiers > 0;
    }
}