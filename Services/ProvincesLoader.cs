using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IProvincesLoader
    {
        State.Provinces Provinces { get; }
        void Save();
        void Reset(State.Settings settings, IReadOnlyList<State.Player> players);
        void Set(IEnumerable<State.Province> new_provinces);
    }
    public class ProvincesLoader : IProvincesLoader
    {
        readonly IServiceIO io;
        Load.ProvincesLoader loader;
        public State.Provinces Provinces { get; private set; }
        public ProvincesLoader(IServiceIO io, ISettingsLoader settings, IPlayersLoader players)
        {
            this.io = io;
            loader = new Load.ProvincesLoader(io.Provinces, io.Graph, settings.Settings, players);
            Provinces = loader.Load();
        }
        public void Save() => loader.Save(Provinces);
        public void Set(IEnumerable<State.Province> new_provinces) => Provinces = Provinces.With(new_provinces.ToArray());
        public void Reset(State.Settings settings, IReadOnlyList<State.Player> players) => loader = new Load.ProvincesLoader(io.Provinces, io.Graph, settings, players);
    }
}