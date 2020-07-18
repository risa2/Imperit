using System.Collections.Generic;

namespace Imperit.Dynamics
{
    public interface IAction
    {
        (IAction[], State.Province) Perform(State.Province province, State.Player active) => (new[] { this }, province);
        (IAction[], State.Player) Perform(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces) => (new[] { this }, player);
        (IAction?, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.Provinces provinces) => (this, true);
        bool Allows(ICommand another, IReadOnlyList<State.Player> players, State.Provinces provinces) => true;
        byte Priority { get; }
    }
}