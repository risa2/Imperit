using System.Collections.Generic;

namespace Imperit.Services
{
    public interface IProvincesLoader
    {
        State.Provinces Provinces { get; set; }
        void Save();
        void Reset(State.Settings settings, IReadOnlyList<State.Player> players);
    }
    public class ProvincesLoader : IProvincesLoader
    {
        readonly IServiceIO io;
        Load.ProvincesLoader loader;
        public State.Provinces Provinces { get; set; }
        public ProvincesLoader(IServiceIO io, ISettingsLoader settings, IPlayersLoader players)
        {
            this.io = io;
            loader = new Load.ProvincesLoader(io.Provinces, io.Graph, settings.Settings, players);
            Provinces = loader.Load();
        }
        public void Save() => loader.Save(Provinces);
        public void Reset(State.Settings settings, IReadOnlyList<State.Player> players) => loader = new Load.ProvincesLoader(io.Provinces, io.Graph, settings, players);
    }
}