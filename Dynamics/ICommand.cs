using System.Collections.Generic;

namespace Imperit.Dynamics
{
    public interface ICommand
    {
        (IAction[], State.Province) Perform(State.Province province) => (System.Array.Empty<IAction>(), province);
        (IAction[], State.Player) Perform(State.Player player, State.IProvinces provinces) => (System.Array.Empty<IAction>(), player);
        bool Allowed(IReadOnlyList<State.Player> players, State.IProvinces provinces);
    }
}
