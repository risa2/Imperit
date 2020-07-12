using System.Collections.Generic;

namespace Imperit.Dynamics
{
    public interface ICommand
    {
        (IAction[], State.Province) Do(State.Province province) => (new IAction[0], province);
        (IAction[], State.Player) Do(State.Player player, State.Provinces provinces) => (new IAction[0], player);
        bool Allowed(IReadOnlyList<State.Player> players, State.Provinces provinces);
    }
}
