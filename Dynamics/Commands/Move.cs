using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public abstract class Move : ICommand
    {
        public readonly int Player, From, To;
        public readonly State.IArmy Army;
        public Move(int player, int from, int to, State.IArmy army)
        {
            Player = player;
            From = from;
            To = to;
            Army = army;
        }
        public bool Allowed(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces)
            => provinces[From].IsControlledBy(players[Player]) && provinces.CanMove(provinces[From], provinces[To]) >= Army.Soldiers && Army.Soldiers > 0;
        public abstract IAction Do(State.Settings settings, IArray<State.Player> players, State.Provinces provinces);
        public uint Soldiers => Army.Soldiers;
    }
}