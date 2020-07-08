using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics
{
    public class ActionQueue : IEnumerable<IAction>
    {
        readonly List<IAction> actions;
        public ActionQueue(List<IAction> actions) => this.actions = actions;
        static IEnumerable<IAction> DoActions(IArray<State.Player> players, State.Provinces provinces, int active, IEnumerable<IAction> actions) => actions.SelectMany(action => ToInsert(action.Do(players, provinces, active)));
        static IEnumerable<IAction> Flatten(Actions.Combination combination) => combination.Actions.SelectMany(action => action is Actions.Combination c ? Flatten(c) : action is Actions.Nothing ? new IAction[0] : new[] { action });
        static IEnumerable<IAction> ToInsert(IAction action) => Flatten(new Actions.Combination(action));
        void AddAction(IAction action) => actions.AddRange(ToInsert(action));
        public IEnumerator<IAction> GetEnumerator() => actions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => actions.GetEnumerator();
        bool Interact(ICommand command)
        {
            bool ok = true;
            for (int i = 0; i < actions.Count && ok; ++i)
            {
                (actions[i], ok) = actions[i].Interact(command);
            }
            return ok;
        }
        bool IsAllowed(ICommand command) => actions.All(action => action.Allows(command));
        void DoCommand(State.Settings settings, IArray<State.Player> players, State.Provinces provinces, ICommand command)
        {
            var actions = command.Do(settings, players, provinces);
            if (Interact(command))
            {
                AddAction(actions);
            }
        }
        public ActionQueue EndOfTurn(IArray<State.Player> players, State.Provinces provinces, int active) => new ActionQueue(DoActions(players, provinces, active, actions.OrderBy(x => x.Priority)).ToList());
        public bool Add(State.Settings settings, IArray<State.Player> players, State.Provinces provinces, ICommand command)
        {
            bool success = command.Allowed(settings, players, provinces) && IsAllowed(command);
            if (success)
            {
                DoCommand(settings, players, provinces, command);
            }
            return success;
        }
    }
}