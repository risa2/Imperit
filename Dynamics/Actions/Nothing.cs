using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class Nothing : IAction
    {
        public IAction Do(IList<State.Player> players, State.Provinces provinces, int active) => this;
    }
}