using System.Collections.Generic;

namespace Imperit.Services
{
    public interface IActionReader
    {
        IEnumerable<Dynamics.IAction> Actions { get; }
    }
    public class ActionReader : IActionReader
    {
        ISettingsLoader sl;
        IPlayersLoader players;
        IProvincesLoader pr;
        Load.IFile inout;
        public ActionReader(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader pr, IServiceIO io)
        {
            this.sl = sl;
            this.players = players;
            this.pr = pr;
            this.inout = io.Actions;
        }
        public IEnumerable<Dynamics.IAction> Actions => new Load.Writer<Load.Action, Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>(inout, (sl.Settings, players, pr.Provinces), Load.Action.FromAction).Load();
    }
}