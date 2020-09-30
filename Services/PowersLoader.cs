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
		uint[] SoldiersOf(IReadOnlyCollection<Player> players)
		{
			return players.Select(player => (uint)provinces.Where(prov => prov.IsControlledBy(player.Id))
														   .Sum(prov => prov.Soldiers.Price)).ToArray();
		}
		static double Div(double a, double b) => (a / b) switch
		{
			double.NaN => 0.0,
			double.PositiveInfinity => double.MaxValue,
			double.NegativeInfinity => double.MinValue,
			_ => a / b
		};
		public void Add(IReadOnlyCollection<Player> players)
		{
			var soldiers = SoldiersOf(players);
			var totals = players.Select((p, i) => (long)soldiers[i] + p.Money + (p.Income * 5)).ToArray();
			var changes = players.Select((p, i) => powers.Any() ? Div(totals[i], powers.Last()[i].Total) - 1 : 0.0).ToArray();
			var sum_sm = players.Select((p, i) => (p, i)).Where(x => !(x.p is Savage)).Sum(x => (long)soldiers[x.i] + x.p.Money);
			var ratios = players.Select((p, i) => Div(soldiers[i] + p.Money, sum_sm)).ToArray();
			powers.Add(players.Select((p, i) => new PlayerPower(totals[i], changes[i], ratios[i])).ToImmutableArray());
			loader.Add(powers.Last());
		}
		public IEnumerator<ImmutableArray<PlayerPower>> GetEnumerator() => powers.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}