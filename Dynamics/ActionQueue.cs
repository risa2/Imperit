using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics
{
    public class ActionQueue : IEnumerable<IAction>
    {
        private List<IAction> actions;
        public ActionQueue(List<IAction> actions) => this.actions = actions;
        private static IEnumerable<IAction> Flatten(Actions.Combination combination) => combination.Actions.SelectMany(action => action is Actions.Combination c ? Flatten(c) : action is Actions.Nothing ? new IAction[0] : new[] { action });
        private static IEnumerable<IAction> ToInsert(IAction action) => Flatten(new Actions.Combination(action));
        private void AddAction(IAction action) => actions.AddRange(ToInsert(action));
        public ActionQueue EndOfTurn(IList<State.Player> players, State.Provinces provinces, int active) => new ActionQueue(actions.OrderBy(x => x.Priority).SelectMany(action => ToInsert(action.Do(players, provinces, active))).ToList());
        public bool Add(ICommand command, IList<State.Player> players, State.Provinces provinces)
        {
            bool success = command.Allowed && actions.All(action => action.IsOkWith(command));
            if (success)
            {
                command.Do(players, provinces);
                ICommand? after_interaction = command;
                for (int i = 0; i < actions.Count && after_interaction != null; ++i)
                {
                    (actions[i], after_interaction) = actions[i].Interact(after_interaction);
                }
                if (after_interaction != null)
                {
                    AddAction(command.Consequences);
                }
            }
            return success;
        }

        public IEnumerator<IAction> GetEnumerator() => actions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => actions.GetEnumerator();
    }
}