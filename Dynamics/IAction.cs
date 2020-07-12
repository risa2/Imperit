using System.Collections.Generic;

namespace Imperit.Dynamics
{
    public interface IAction
    {
        (IAction? NewThis, IAction[] Side, State.Province) Do(State.Province province, State.Player active) => (this, new IAction[0], province);
        (IAction? NewThis, IAction[] Side, State.Player) Do(State.Player player, State.Player active, IReadOnlyList<State.Province> provinces) => (this, new IAction[0], player);
        (IAction, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.Provinces provinces) => (this, true);
        bool Allows(ICommand another, IReadOnlyList<State.Player> players, State.Provinces provinces) => true;
        byte Priority { get; }
    }
}