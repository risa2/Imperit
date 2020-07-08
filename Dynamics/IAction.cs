namespace Imperit.Dynamics
{
    public interface IAction
    {
        IAction Do(IArray<State.Player> players, State.Provinces provinces, int active);
        (IAction, bool) Interact(ICommand another) => (this, true);
        bool Allows(ICommand another) => true;
        int Priority => 0;
    }
}