using System;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IActionWriter
    {
        bool Add(IEnumerable<Dynamics.ICommand> commands);
        bool Add(params Dynamics.ICommand[] commands) => Add(commands as IEnumerable<Dynamics.ICommand>);
        void ApplyActions(IList<State.Player> players, State.Provinces provinces, int active, Func<Dynamics.IAction, bool> cond);
        void EndOfTurn(int active);
        void NewGame();
        void StartGame(IEnumerable<Dynamics.IAction> actions);
    }
    public class ActionWriter : IActionWriter
    {
        readonly IPlayersLoader players;
        readonly IProvincesLoader pr;
        readonly Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)> action_loader;
        readonly Load.Writer<Load.Command, Dynamics.ICommand, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)> event_loader;
        Dynamics.ActionQueue queue;
        public ActionWriter(ISettingsLoader sl, IPlayersLoader pl, IProvincesLoader pr, IServiceIO io)
        {
            this.players = pl;
            this.pr = pr;
            action_loader = new Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>(io.Actions, (sl.Settings, players, pr.Provinces), Load.Action.FromAction);
            event_loader = new Load.Writer<Load.Command, Dynamics.ICommand, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>(io.Events, (sl.Settings, players, pr.Provinces), Load.Command.FromCommand);
            queue = new Dynamics.ActionQueue(new List<Dynamics.IAction>(action_loader.Load()));
        }
        public bool Add(IEnumerable<Dynamics.ICommand> commands)
        {
            bool success = false;
            foreach(var command in commands)
            {
                success |= queue.Add(players, pr.Provinces, command);
            }
            if (success)
            {
                pr.Save();
                action_loader.Save(queue);
                event_loader.Add(commands);
            }
            return success;
        }
        void SaveActionQueue(Dynamics.ActionQueue new_queue)
        {
            queue = new_queue;
            action_loader.Save(queue);
        }
        public void ApplyActions(IList<State.Player> players, State.Provinces provinces, int active, Func<Dynamics.IAction, bool> cond) => new Dynamics.ActionQueue(queue.Where(cond).ToList()).EndOfTurn(players, provinces, active);
        public void EndOfTurn(int active) => SaveActionQueue(queue.EndOfTurn(players, pr.Provinces, active));
        public void NewGame()
        {
            SaveActionQueue(new Dynamics.ActionQueue(new List<Dynamics.IAction>()));
            event_loader.Save(new Dynamics.ICommand[0]);
        }
        public void StartGame(IEnumerable<Dynamics.IAction> actions) => SaveActionQueue(new Dynamics.ActionQueue(new List<Dynamics.IAction>(actions)));
    }
}