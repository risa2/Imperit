using System.Collections.Generic;

namespace Imperit.Dynamics
{
    public interface ICommand
    {
        void Do(IList<State.Player> players, State.Provinces provinces);
        IAction Consequences { get; }
        bool Allowed { get; }
    }
}
