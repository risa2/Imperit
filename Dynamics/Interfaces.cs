using System.Collections.Generic;

namespace Imperit.Dynamics
{
    public interface IAction
    {
        IAction Do(IList<State.Player> players, State.Provinces provinces, int active);
        (IAction, ICommand?) Interact(ICommand another) => (this, another);
        bool IsOkWith(ICommand another) => true;
        int Priority => 0;
    }
    public interface ICommand
    {
        void Do(IList<State.Player> players, State.Provinces provinces);
        IAction Consequences { get; }
        bool Allowed { get; }
    }
}