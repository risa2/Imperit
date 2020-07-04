using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics
{
    public class ActionQueue : IEnumerable<IAction>
    {
        readonly List<IAction> actions;
        public ActionQueue(List<IAction> actions) => this.actions = actions;
        static IEnumerable<IAction> Flatten(Actions.Combination combination) => combination.Actions.SelectMany(action => action is Actions.Combination c ? Flatten(c) : action is Actions.Nothing ? new IAction[0] : new[] { action });
        static IEnumerable<IAction> ToInsert(IAction action) => Flatten(new Actions.Combination(action));
        void AddAction(IAction action) => actions.AddRange(ToInsert(action));
        public ActionQueue EndOfTurn(IList<State.Player> players, State.Provinces provinces, int active) => new ActionQueue(actions.OrderBy(x => x.Priority).SelectMany(action => ToInsert(action.Do(players, provinces, active))).ToList());
        public bool Add(State.Settings settings, IArray<State.Player> players, State.Provinces provinces, ICommand command)
        {
            bool success = command.Allowed(settings, players, provinces) && actions.All(action => action.IsOkWith(command));
            if (success)
            {
                var consequences = command.Do(settings, players, provinces);
                bool valid_command = true;
                for (int i = 0; i < actions.Count && valid_command; ++i)
                {
                    (actions[i], valid_command) = actions[i].Interact(command);
                }
                if (valid_command)
                {
                    AddAction(consequences);
                }
            }
            return success;
        }

        public IEnumerator<IAction> GetEnumerator() => actions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => actions.GetEnumerator();
    }
}