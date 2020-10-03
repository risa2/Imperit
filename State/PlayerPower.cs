using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.State
{
	public readonly struct PlayerPower : System.IEquatable<PlayerPower>
	{
		public readonly long Total;
		public readonly double Change, Ratio;
		public PlayerPower(long total, double change, double ratio)
		{
			Total = total;
			Change = change;
			Ratio = ratio;
		}
		public override bool Equals(object? obj) => obj is PlayerPower pp && pp.Equals(this);
		public override int GetHashCode() => (Total, Change, Ratio).GetHashCode();
		public static bool operator ==(PlayerPower left, PlayerPower right) => left.Equals(right);
		public static bool operator !=(PlayerPower left, PlayerPower right) => !left.Equals(right);
		public bool Equals(PlayerPower other) => (Total, Change, Ratio) == (other.Total, other.Change, other.Ratio);
		public static ImmutableArray<PlayerPower> Compute(IProvinces provinces, IReadOnlyCollection<Player> players, IReadOnlyList<PlayerPower>? last)
		{
			static double Div(double a, double b) => (a / b) switch
			{
				double.NaN => 0.0,
				double.PositiveInfinity => double.MaxValue,
				double.NegativeInfinity => double.MinValue,
				_ => a / b
			};
			static (long Soldiers, long Income) SoldiersIncome(IEnumerable<Province> provinces)
			{
				return provinces.Aggregate((0L, 0L), (pair, prov) => (pair.Item1 + prov.Soldiers.Price, pair.Item2 + prov.Earnings));
			}
			var pairs = players.Select(player => SoldiersIncome(provinces.Where(prov => prov.IsControlledBy(player.Id)))).ToArray();
			var totals = players.Zip(pairs, (p, pair) => pair.Soldiers + p.Money + (pair.Income * 5)).ToArray();
			var changes = last is null ? 0.0.Infinity() : players.Select((p, i) => Div(totals[i], last[i].Total) - 1);
			var sum_sm = players.Enumerate().Where(x => !(x.v is Savage)).Sum(x => pairs[x.i].Soldiers + x.v.Money);
			var ratios = players.Zip(pairs, (p, pair) => Div(pair.Soldiers + p.Money, sum_sm));
			return totals.Zip(ratios.Zip(changes), (t, p) => new PlayerPower(t, p.Second, p.First)).ToImmutableArray();
		}
	}
}