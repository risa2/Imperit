using System.Collections.Generic;

namespace Imperit.Services
{
    public interface IProvincesLoader
    {
        State.Provinces Provinces { get; set; }
        void Save();
        void NewGame(ISettingsLoader settings, IPlayersLoader players);
    }
    public class ProvincesLoader : IProvincesLoader
    {
        readonly IServiceIO io;
        Load.ProvincesLoader loader;
        public State.Provinces Provinces { get; set; }
        public ProvincesLoader(IServiceIO io, ISettingsLoader settings, IPlayersLoader players)
        {
            this.io = io;
            loader = new Load.ProvincesLoader(io.Provinces, io.Graph, settings.Settings, players as IReadOnlyList<State.Player>);
            Provinces = loader.Load();
        }
        public void Save() => loader.Save(Provinces);
        public void NewGame(ISettingsLoader settings, IPlayersLoader players) => loader = new Load.ProvincesLoader(io.Provinces, io.Graph, settings.Settings, players);
    }
}