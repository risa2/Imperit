using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Recruitment : ICommand
    {
        public readonly int Player, Land;
        public readonly uint Soldiers;
        public Recruitment(int player, int land, uint soldiers)
        {
            Player = player;
            Land = land;
            Soldiers = soldiers;
        }
        public IAction Do(State.Settings settings, IArray<State.Player> players, State.Provinces provinces)
        {
            players[Player] = players[Player].Pay(Soldiers);
            return new Actions.Reinforcement(Land, new State.PlayerArmy(settings, players[Player], Soldiers), players);
        }
        public bool Allowed(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces) => provinces[Land].IsControlledBy(players[Player]) && players[Player].Money >= Soldiers && Soldiers > 0;
    }
}