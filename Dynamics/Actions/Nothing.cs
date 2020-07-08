namespace Imperit.Dynamics.Actions
{
    public class Nothing : IAction
    {
        public IAction Do(IArray<State.Player> players, State.Provinces provinces, int active) => this;
    }
}