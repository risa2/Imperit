using System;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IActionWriter
    {
        bool Add(IEnumerable<Dynamics.ICommand> commands, bool save);
        bool Add(Dynamics.ICommand command) => Add(new[] { command }, true);
        Dynamics.ActionQueue Where(Func<Dynamics.IAction, bool> cond);
        void EndOfTurn(int active);
        void Clear();
        void Save();
        void Save(Dynamics.ActionQueue queue);
    }
    public class ActionWriter : IActionWriter
    {
        readonly IPlayersLoader players;
        readonly IProvincesLoader pr;
        readonly Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)> loader;
        Dynamics.ActionQueue queue;
        public ActionWriter(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader pr, IServiceIO io)
        {
            this.players = players;
            this.pr = pr;
            loader = new Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>(io.Actions, (sl.Settings, players, pr.Provinces), Load.Action.FromAction);
            queue = new Dynamics.ActionQueue(loader.Load());
        }
        public bool Add(IEnumerable<Dynamics.ICommand> commands, bool save)
        {
            bool changed = false;
            foreach (var command in commands)
            {
                (var new_queue, var new_players, var new_provinces, bool ch) = queue.Add(command, players, pr.Provinces);
                queue = new_queue;
                if (ch)
                {
                    pr.Set(new_provinces);
                    players.Set(new_players);
                    changed = true;
                }
            }
            if (save && changed)
            {
                players.Save();
                pr.Save();
                Save();
            }
            return changed;
        }
        public void Save(Dynamics.ActionQueue new_queue) => loader.Save(queue = new_queue);
        public Dynamics.ActionQueue Where(Func<Dynamics.IAction, bool> cond) => new Dynamics.ActionQueue(queue.Where(cond));
        public void EndOfTurn(int active)
        {
            (var new_queue, var new_players, var new_provinces) = queue.EndOfTurn(players, pr.Provinces, active);
            queue = new_queue;
            pr.Set(new_provinces.ToArray());
            players.Set(new_players);
        }
        public void Clear() => Save(new Dynamics.ActionQueue());
        public void Save() => Save(queue);
    }
}