using System.Collections.Generic;

namespace Imperit.Dynamics
{
    public interface ICommand
    {
        IAction Do(State.Settings settings, IArray<State.Player> players, State.Provinces provinces);
        bool Allowed(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces);
    }
}
