using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Imperit.State;

namespace Imperit.Services
{
	public interface IProvincesLoader : IProvinces
	{
		void Save();
		void Reset(Settings settings, IReadOnlyList<Player> players);
		void Set(IEnumerable<Province> new_provinces);
	}
	public class ProvincesLoader : IProvincesLoader
	{
		readonly IServiceIO io;
		Load.ProvincesLoader loader;
		Provinces provinces;
		public ProvincesLoader(IServiceIO io, ISettingsLoader settings, IPlayersLoader players)
		{
			this.io = io;
			loader = new Load.ProvincesLoader(io.Provinces, io.Graph, io.Shapes, settings.Settings, players);
			provinces = loader.Load();
		}
		public IEnumerator<Province> GetEnumerator() => provinces.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public bool Passable(int from, int to) => provinces.Passable(from, to);
		public uint? Distance(int from, int to) => provinces.Distance(from, to);
		public uint NeighborCount(int id) => provinces.NeighborCount(id);
		public IEnumerable<Province> NeighborsOf(int id) => provinces.NeighborsOf(id);
		public int Count => provinces.Count;
		public Province this[int i]
		{
			get => provinces[i];
			set => provinces[i] = value;
		}
		public void Save() => loader.Save(provinces);
		public Provinces With(Province[] new_provinces) => provinces.With(new_provinces);
		public void Set(IEnumerable<Province> new_provinces) => provinces = With(new_provinces.ToArray());
		public void Reset(Settings settings, IReadOnlyList<Player> players) => loader = new Load.ProvincesLoader(io.Provinces, io.Graph, io.Shapes, settings, players);
	}
}