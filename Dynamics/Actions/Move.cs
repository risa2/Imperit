using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public abstract class Move : IAction
    {
        public readonly int Province;
        public readonly State.IArmy Army;
        public Move(int province, State.IArmy army)
        {
            Province = province;
            Army = army;
        }
        public abstract IAction Do(IList<State.Player> players, State.Provinces provinces, int active);
        public abstract (IAction, bool) Interact(ICommand another);
        public uint Soldiers => Army.Soldiers;
    }
}