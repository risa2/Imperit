using System.Collections.Generic;

namespace Imperit.Services
{
    public interface ICommandReader
    {
        IEnumerable<Dynamics.ICommand> Events { get; }
    }
    public class CommandReader : ICommandReader
    {
        ISettingsLoader sl;
        IPlayersLoader players;
        IProvincesLoader pr;
        Load.IFile inout;
        public CommandReader(ISettingsLoader sl, IPlayersLoader pl, IProvincesLoader pr, IServiceIO io)
        {
            this.sl = sl;
            this.players = pl;
            this.pr = pr;
            this.inout = io.Events;
        }
        public IEnumerable<Dynamics.ICommand> Events => new Load.Writer<Load.Command, Dynamics.ICommand, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>(inout, (sl.Settings, players, pr.Provinces), Load.Command.FromCommand).Load();
    }
}