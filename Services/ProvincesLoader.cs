using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IProvincesLoader : State.IProvinces
    {
        void Save();
        void Reset(State.Settings settings, IReadOnlyList<State.Player> players);
        void Set(IEnumerable<State.Province> new_provinces);
    }
    public class ProvincesLoader : IProvincesLoader
    {
        readonly IServiceIO io;
        Load.ProvincesLoader loader;
        State.Provinces provinces;
        public ProvincesLoader(IServiceIO io, ISettingsLoader settings, IPlayersLoader players)
        {
            this.io = io;
            loader = new Load.ProvincesLoader(io.Provinces, io.Graph, io.Shapes, settings.Settings, players);
            provinces = loader.Load();
        }
        public IEnumerator<State.Province> GetEnumerator() => provinces.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public uint CanMove(int from, int to) => provinces.CanMove(from, to);
        public uint NeighborCount(int id) => provinces.NeighborCount(id);
        public IEnumerable<State.Province> NeighborsOf(int id) => provinces.NeighborsOf(id);
        public int Count => provinces.Count;
        public State.Province this[int i]
        {
            get => provinces[i];
            set => provinces[i] = value;
        }
        public void Save() => loader.Save(provinces);
        public void Set(IEnumerable<State.Province> new_provinces) => provinces = provinces.With(new_provinces.ToArray());
        public void Reset(State.Settings settings, IReadOnlyList<State.Player> players) => loader = new Load.ProvincesLoader(io.Provinces, io.Graph, io.Shapes, settings, players);
    }
}