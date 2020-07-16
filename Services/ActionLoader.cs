using System;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IActionLoader
    {
        bool Add(IEnumerable<Dynamics.ICommand> commands);
        bool Add(Dynamics.ICommand command) => Add(new[] { command });
        Dynamics.ActionQueue Where(Func<Dynamics.IAction, bool> cond);
        void EndOfTurn(int active);
        void Clear();
        void Save();
        void Save(Dynamics.ActionQueue Actions);
        Dynamics.ActionQueue Actions { get; }
    }
    public class ActionLoader : IActionLoader
    {
        readonly IPlayersLoader players;
        readonly IProvincesLoader pr;
        readonly Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>)> loader;
        public Dynamics.ActionQueue Actions { get; private set; }
        public ActionLoader(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader pr, IServiceIO io)
        {
            this.players = players;
            this.pr = pr;
            loader = new Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>)>(io.Actions, (sl.Settings, players), Load.Action.FromAction);
            Actions = new Dynamics.ActionQueue(loader.Load());
        }
        public bool Add(IEnumerable<Dynamics.ICommand> commands)
        {
            bool changed = false;
            foreach (var command in commands)
            {
                var (queue, new_players, new_provinces, ch) = Actions.Add(command, players, pr.Provinces);
                Actions = queue;
                if (ch)
                {
                    pr.Set(new_provinces);
                    players.Assign(new_players);
                    changed = true;
                }
            }
            return changed;
        }
        public void Save(Dynamics.ActionQueue queue) => loader.Save(Actions = queue);
        public Dynamics.ActionQueue Where(Func<Dynamics.IAction, bool> cond) => new Dynamics.ActionQueue(Actions.Where(cond));
        public void EndOfTurn(int active)
        {
            var (queue, new_players, new_provinces) = Actions.EndOfTurn(players, pr.Provinces, active);
            Actions = queue;
            pr.Set(new_provinces.ToArray());
            players.Assign(new_players);
        }
        public void Clear() => Save(new Dynamics.ActionQueue());
        public void Save() => Save(Actions);
    }
}