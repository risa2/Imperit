using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public abstract class Move : ICommand
    {
        public readonly int Player, From;
        public readonly State.Province To;
        public readonly State.IArmy Army;
        public Move(int player, int from, State.Province to, State.IArmy army)
        {
            Player = player;
            From = from;
            To = to;
            Army = army;
        }
        public bool Allowed(IReadOnlyList<State.Player> players, State.Provinces provinces)
            => provinces[From].IsControlledBy(Player) && provinces.CanMove(provinces[From], To) >= Army.Soldiers && Army.Soldiers > 0;
        protected abstract Actions.Move GetMove();
        public (IAction[], State.Province) Perform(State.Province province)
        {
            return province.Id == From ? (new[] { GetMove() }, province.StartMove(To, Army)) : (System.Array.Empty<IAction>(), province);
        }
        public uint Soldiers => Army.Soldiers;
    }
}