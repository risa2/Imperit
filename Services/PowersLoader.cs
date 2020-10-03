using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Imperit.State;

namespace Imperit.Services
{
	public interface IPowersLoader : IReadOnlyList<ImmutableArray<PlayerPower>>
	{
		void Add(IReadOnlyCollection<Player> players);
		void Clear();
	}
	public class PowersLoader : IPowersLoader
	{
		readonly IProvincesLoader provinces;
		readonly Load.JsonWriter<Load.JsonPlayersPower, ImmutableArray<PlayerPower>, bool> loader;
		readonly List<ImmutableArray<PlayerPower>> powers;
		public int Count => powers.Count;
		public ImmutableArray<PlayerPower> this[int i] => powers[i];
		public PowersLoader(IServiceIO io, IProvincesLoader provinces)
		{
			loader = new Load.JsonWriter<Load.JsonPlayersPower, ImmutableArray<PlayerPower>, bool>(io.Powers, false, Load.JsonPlayersPower.From);
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
			var values = PlayerPower.Compute(provinces, players, powers.Any() ? powers?.Last() : null);
			powers!.Add(values);
			loader.Add(values);
		}
		public IEnumerator<ImmutableArray<PlayerPower>> GetEnumerator() => powers.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}