using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public abstract class Move : ICommand
    {
        public State.Player Player { get; }
        protected readonly State.Provinces provinces;
        public readonly State.Province From, To;
        public readonly State.IArmy Army;
        public Move(State.Provinces prov, State.Player player, State.Province from, State.Province to, State.IArmy army)
        {
            Player = player;
            provinces = prov;
            From = from;
            To = to;
            Army = army;
        }
        public bool Allowed => From.IsControlledBy(Player) && provinces.CanMove(From, To) >= Army.Soldiers && Army.Soldiers > 0;
        public void Do(IList<State.Player> players, State.Provinces provinces) => provinces[From.Id] = From.StartMove(To, Army);
        public uint Soldiers => Army.Soldiers;
        public abstract IAction Consequences { get; }
    }
}