using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Recruitment : ICommand
    {
        readonly State.Settings settings;
        public readonly int Player, Land;
        public readonly uint Soldiers;
        public Recruitment(int player, int land, uint soldiers, State.Settings settings)
        {
            Player = player;
            Land = land;
            Soldiers = soldiers;
            this.settings = settings;
        }
        public (IAction[], State.Player) Do(State.Player player, State.Provinces provinces)
        {
            return player.Id == Player
                ? (new[] { new Actions.Reinforcement(Land, new State.PlayerArmy(settings, player, Soldiers)) }, player.Pay(Soldiers))
                : (new IAction[0], player);
        }
        public bool Allowed(IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            return provinces[Land].IsControlledBy(players[Player]) && players[Player].Money >= Soldiers && Soldiers > 0;
        }
    }
}