using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public abstract class ArmyOperation : IAction
    {
        public readonly int Province;
        public readonly State.IArmy Army;
        public ArmyOperation(int province, State.IArmy army)
        {
            Province = province;
            Army = army;
        }
        public abstract (IAction[], State.Province) Perform(State.Province province, State.Player active);
        public abstract (IAction, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.IProvinces provinces);
        public uint Soldiers => Army.Soldiers;
        public byte Priority => 50;
    }
}