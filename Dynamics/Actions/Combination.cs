using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics.Actions
{
    public class Combination : IAction
    {
        public readonly IEnumerable<IAction> Actions;
        public Combination(IEnumerable<IAction> actions) => Actions = actions;
        public Combination(params IAction[] actions) => Actions = actions;
        public IAction Do(IList<State.Player> players, State.Provinces provinces, int active) => new Combination(Actions.Select(action => action.Do(players, provinces, active)).Where(action => !(action is Nothing)));
    }
}