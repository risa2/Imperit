using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.State
{
	public class PlayerPower
	{
		public readonly bool Alive;
		public readonly int Soldiers, Lands, Income, Money;
		public PlayerPower(bool alive, int soldiers, int lands, int income, int money)
		{
			Alive = alive;
			Soldiers = soldiers;
			Lands = lands;
			Income = income;
			Money = money;
		}

		public int Total => Alive ? Soldiers + Money + (Income * 5) : 0;
	}
	public class PlayersPower : IReadOnlyList<PlayerPower>
	{
		readonly ImmutableArray<PlayerPower> pp;
		public PlayersPower(ImmutableArray<PlayerPower> pp) => this.pp = pp;
		public PlayerPower this[int index] => pp[index];
		public int Count => pp.Length;
		public IEnumerator<PlayerPower> GetEnumerator() => ((IEnumerable<PlayerPower>)pp).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public ImmutableArray<double> GetRatios()
		{
			var sum = pp.Sum(p => p.Soldiers + p.Money);
			return pp.Select(p => (double)(p.Soldiers + p.Money) / sum).ToImmutableArray();
		}
		public int SoldiersSum => pp.Sum(p => p.Soldiers);
		public int LandsSum => pp.Sum(p => p.Lands);
		public int IncomeSum => pp.Sum(p => p.Income);
		public int MoneySum => pp.Sum(p => p.Money);
		public int TotalSum => pp.Sum(p => p.Total);
		public int TotalAvg => TotalSum / Count;
		public int TotalMax => pp.Max(pp => pp.Total);
		public bool MajorityReached => pp.Any(pp => pp.Lands > 2 || pp.Soldiers * 2 > SoldiersSum && pp.Lands * 2 > LandsSum && pp.Income * 2 > IncomeSum && pp.Money * 2 > MoneySum);
		public static PlayersPower Compute(IProvinces provinces, IReadOnlyCollection<Player> players)
		{
			static (int Soldiers, int Income, int Lands) SoldiersIncome(IEnumerable<Province> provinces)
			{
				return provinces.Aggregate((0, 0, 0), (pair, prov) => (pair.Item1 + prov.Soldiers.Price, pair.Item2 + prov.Earnings, pair.Item3 + 1));
			}
			var pairs = players.Select(player => (SoldiersIncome(provinces.ControlledBy(player.Id)), player.Alive && !(player is Savage), player.Money));
			return new PlayersPower(pairs.Select(it => it.Item2 ? new PlayerPower(true, it.Item1.Soldiers, it.Item1.Lands, it.Item1.Income, it.Money) : new PlayerPower(false, 0, 0, 0, 0)).ToImmutableArray());
		}
	}
}