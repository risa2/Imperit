using System;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IActionWriter
    {
        bool Add(IEnumerable<Dynamics.ICommand> commands, bool save);
        bool Add(Dynamics.ICommand command) => Add(new[] { command }, true);
        void ApplyActions(IList<State.Player> players, State.Provinces provinces, int active, Func<Dynamics.IAction, bool> cond);
        void EndOfTurn(int active);
        void Clear();
        void Save();
        void Save(Dynamics.ActionQueue queue);
    }
    public class ActionWriter : IActionWriter
    {
        readonly ISettingsLoader sl;
        readonly IPlayersLoader players;
        readonly IProvincesLoader pr;
        readonly Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)> loader;
        Dynamics.ActionQueue queue;
        public ActionWriter(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader pr, IServiceIO io)
        {
            this.players = players;
            this.pr = pr;
            this.sl = sl;
            loader = new Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>(io.Actions, (sl.Settings, players, pr.Provinces), Load.Action.FromAction);
            queue = new Dynamics.ActionQueue(new List<Dynamics.IAction>(loader.Load()));
        }
        public bool Add(IEnumerable<Dynamics.ICommand> commands, bool save)
        {
            bool success = false;
            foreach(var command in commands)
            {
                success |= queue.Add(sl.Settings, players, pr.Provinces, command);
            }
            if (save && success)
            {
                players.Save();
                pr.Save();
                Save();
            }
            return success;
        }
        public void Save(Dynamics.ActionQueue new_queue) => loader.Save(queue = new_queue);
        public void ApplyActions(IList<State.Player> players, State.Provinces provinces, int active, Func<Dynamics.IAction, bool> cond) => new Dynamics.ActionQueue(queue.Where(cond).ToList()).EndOfTurn(players, provinces, active);
        public void EndOfTurn(int active) => queue = queue.EndOfTurn(players, pr.Provinces, active);
        public void Clear() => Save(new Dynamics.ActionQueue(new List<Dynamics.IAction>()));
        public void Save() => Save(queue);
    }
}