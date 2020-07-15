using System.Collections.Generic;

namespace Imperit.Dynamics
{
    public interface ICommand
    {
        (IAction[], State.Province) Do(State.Province province) => (System.Array.Empty<IAction>(), province);
        (IAction[], State.Player) Do(State.Player player, State.Provinces provinces) => (System.Array.Empty<IAction>(), player);
        bool Allowed(IReadOnlyList<State.Player> players, State.Provinces provinces);
    }
}
