using System.Collections.Generic;

namespace Imperit.Dynamics
{
    public interface IAction
    {
        (IAction[], State.Province) Perform(State.Province province, State.Player active) => (new[] { this }, province);
        (IAction[], State.Player) Perform(State.Player player, State.Player active, State.IProvinces provinces) => (new[] { this }, player);
        (IAction?, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.IProvinces provinces) => (this, true);
        bool Allows(ICommand another, IReadOnlyList<State.Player> players, State.IProvinces provinces) => true;
        byte Priority { get; }
    }
}