using System;
using System.Collections.Generic;
using System.Linq;
using Imperit.State;

namespace Imperit.Services
{
    public interface IActionWriter
    {
        bool Add(Dynamics.ICommand command);
        void ApplyActions(IList<Player> players, Provinces provinces, int active, Func<Dynamics.IAction, bool> cond);
        void EndOfTurn(int active);
        void NewGame();
        void StartGame(IEnumerable<Dynamics.IAction> actions);
    }
    public class ActionWriter : IActionWriter
    {
        ISettingsLoader sl;
        IPlayersLoader players;
        IProvincesLoader pr;
        Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)> action_loader;
        Load.Writer<Load.Command, Dynamics.ICommand, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)> event_loader;
        Dynamics.ActionQueue queue;
        public ActionWriter(ISettingsLoader sl, IPlayersLoader pl, IProvincesLoader pr, IServiceIO io)
        {
            this.sl = sl;
            this.players = pl;
            this.pr = pr;
            action_loader = new Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>(io.Actions, (sl.Settings, players, pr.Provinces), Load.Action.FromAction);
            event_loader = new Load.Writer<Load.Command, Dynamics.ICommand, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>(io.Events, (sl.Settings, players, pr.Provinces), Load.Command.FromCommand);
            queue = new Dynamics.ActionQueue(new List<Dynamics.IAction>(action_loader.Load()));
        }
        public bool Add(Dynamics.ICommand command)
        {
            bool success = queue.Add(command, players, pr.Provinces);
            if (success)
            {
                pr.Save();
                action_loader.Save(queue);
                event_loader.Add(command);
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