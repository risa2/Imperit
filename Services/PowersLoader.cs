using Imperit.State;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
	public interface IPowersLoader : IReadOnlyList<PlayersPower>
	{
		void Add(IReadOnlyCollection<Player> players);
		void Clear();
		PlayersPower Last { get; }
	}
	public class PowersLoader : IPowersLoader
	{
		readonly IProvincesLoader provinces;
		readonly Load.JsonWriter<Load.JsonPlayersPower, PlayersPower, bool> loader;
		readonly List<PlayersPower> powers;
		public int Count => powers.Count;

		public PlayersPower Last => powers[^1];
		public PlayersPower this[int i] => powers[i];
		public PowersLoader(IServiceIO io, IProvincesLoader provinces)
		{
			loader = new Load.JsonWriter<Load.JsonPlayersPower, PlayersPower, bool>(io.Powers, false, Load.JsonPlayersPower.From);
			powers = loader.Load().ToList();
			this.provinces = provinces;
		}
		public void Clear()
		{
			loader.Clear();
			powers.Clear();
		}
		public void Add(IReadOnlyCollection<Player> players)
		{
			var values = PlayersPower.Compute(provinces, players);
			powers!.Add(values);
			loader.Add(values);
		}
		public IEnumerator<PlayersPower> GetEnumerator() => powers.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}