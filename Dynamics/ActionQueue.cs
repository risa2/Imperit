using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ActionList = System.Collections.Immutable.ImmutableList<Imperit.Dynamics.IAction>;

namespace Imperit.Dynamics
{
    public class ActionQueue : IEnumerable<IAction>
    {
        readonly ActionList actions;
        public ActionQueue() => actions = ActionList.Empty;
        public ActionQueue(ActionList actions) => this.actions = actions;
        public ActionQueue(IEnumerable<IAction> actions) => this.actions = ImmutableList.CreateRange(actions);
        static (ActionList Actions, ActionList Sides, State.Province Province) ApplyAll(IReadOnlyList<IAction> actions, State.Province province, State.Player active)
        {
            return actions.Aggregate((ActionList.Empty, ActionList.Empty, province), (acc, action) =>
            {
                (var new_action, var side, var new_province) = action.Do(acc.province, active);
                return (new_action is null ? acc.Item1 : acc.Item1.Add(new_action), acc.Item2.AddRange(side), new_province);
            });
        }
        static (ActionList Actions, ActionList Sides, State.Player Player) ApplyAll(IReadOnlyList<IAction> actions, State.Player player, State.Player active, IReadOnlyList<State.Province> provinces)
        {
            return actions.Aggregate((ActionList.Empty, ActionList.Empty, player), (acc, action) =>
            {
                (var new_action, var side, var new_player) = action.Do(acc.player, active, provinces);
                return (new_action is null ? acc.Item1 : acc.Item1.Add(new_action), acc.Item2.AddRange(side), new_player);
            });
        }
        static (IReadOnlyList<IAction>, ActionList, State.Province[]) ApplyAllToAll(IReadOnlyList<State.Province> provinces, State.Player active, IReadOnlyList<IAction> actions)
        {
            var new_provinces = new State.Province[provinces.Count];
            ActionList sides = ActionList.Empty, sides_single;
            foreach ((int i, var province) in provinces.Enumerate())
            {
                (actions, sides_single, new_provinces[i]) = ApplyAll(actions, province, active);
                sides = sides.AddRange(sides_single);
            }
            return (actions, sides, new_provinces);
        }
        static (IReadOnlyList<IAction>, ActionList, State.Player[]) ApplyAllToAll(IReadOnlyList<State.Player> players, State.Player active, IReadOnlyList<State.Province> provinces, IReadOnlyList<IAction> actions)
        {
            var new_players = new State.Player[players.Count];
            ActionList sides = ActionList.Empty, sides_single;
            foreach ((int i, var player) in players.Enumerate())
            {
                (actions, sides_single, new_players[i]) = ApplyAll(actions, player, active, provinces);
                sides = sides.AddRange(sides_single);
            }
            return (actions, sides, new_players);
        }
        static (ActionQueue, IReadOnlyList<State.Player>, IReadOnlyList<State.Province>) DoActions(IReadOnlyList<State.Player> players, IReadOnlyList<State.Province> provinces, int active, IReadOnlyList<IAction> actions)
        {
            ActionList new_actions = ActionList.Empty, sides, sides_single;
            while (actions.Any())
            {
                actions = actions.SortBy(action => action.Priority);
                (actions, sides, provinces) = ApplyAllToAll(provinces, players[active], actions);
                (actions, sides_single, players) = ApplyAllToAll(players, players[active], provinces, actions);
                sides = sides.AddRange(sides_single);

                new_actions = new_actions.AddRange(actions);
                actions = sides;
            }
            return (new ActionQueue(new_actions), players, provinces);
        }
        public (ActionQueue, IReadOnlyList<State.Player>, IReadOnlyList<State.Province>) EndOfTurn(IReadOnlyList<State.Player> players, IReadOnlyList<State.Province> provinces, int active)
        {
            return DoActions(players, provinces, active, actions);
        }
        (ActionList, bool) Interactions(ICommand command, IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            bool allowed = true;
            var new_actions = ActionList.Empty;
            int i;
            for (i = 0; i < actions.Count && allowed; ++i)
            {
                (var new_action, bool ok) = actions[i].Interact(command, players, provinces);
                new_actions = new_actions.Add(new_action);
                allowed = allowed && ok;
            }
            return (new_actions.AddRange(actions.Skip(i)), allowed);
        }
        bool IsAllowed(ICommand cmd, IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            return cmd.Allowed(players, provinces) && actions.All(action => action.Allows(cmd, players, provinces));
        }
        public (ActionQueue, State.Player[], State.Province[], bool) Add(ICommand command, IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            if (IsAllowed(command, players, provinces))
            {
                (var actions1, var new_players) = players.Select(player => command.Do(player, provinces)).Unzip();
                (var actions2, var new_provinces) = provinces.Select(province => command.Do(province)).Unzip();
                (var new_actions, bool interacted) = Interactions(command, players, provinces);
                return (new ActionQueue(interacted ? new_actions.AddRange(actions1.Flatten()).AddRange(actions2.Flatten()) : new_actions), new_players.ToArray(), new_provinces.ToArray(), true);
            }
            return (this, players.ToArray(), provinces.ToArray(), false);
        }

        public IEnumerator<IAction> GetEnumerator() => actions!.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => actions!.GetEnumerator();
    }
}
