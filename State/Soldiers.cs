using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.State
{
	public class Soldiers : IEnumerable<(SoldierType Type, int Count)>
	{
		readonly ImmutableArray<(SoldierType Type, int Count)> soldiers;
		public Soldiers() => soldiers = ImmutableArray<(SoldierType Type, int Count)>.Empty;
		public Soldiers(ImmutableArray<(SoldierType, int)> list) => soldiers = list;
		public Soldiers(IEnumerable<(SoldierType, int)> list) => soldiers = list.ToImmutableArray();
		public Soldiers(params (SoldierType, int)[] list) => soldiers = list.ToImmutableArray();
		public Soldiers Add(Soldiers s)
		{
			var res = soldiers.ToBuilder();
			res.InsertMatch(s.Where(p => p.Count > 0), (a, b) => a.Type == b.Type, (a, b) => (a.Type, a.Count + b.Count));
			return new Soldiers(res.ToImmutable());
		}
		public bool Contains(Soldiers s)
		{
			return s.soldiers.All(s2 => soldiers.Any(s1 => s1.Type == s2.Type && s1.Count >= s2.Count));
		}
		public Soldiers Subtract(Soldiers s)
		{
			var res = soldiers.ToBuilder();
			res.InsertMatch(s.Where(p => p.Count > 0), (a, b) => a.Type == b.Type, (a, b) => (a.Type, a.Count - b.Count));
			return new Soldiers(res.Where(x => x.Count > 0).ToImmutableArray());
		}
		public Soldiers Multiply(int mul) => new Soldiers(soldiers.Select(s => (s.Type, s.Count * mul)));
		public IEnumerable<SoldierType> Types => soldiers.Select(p => p.Type);
		public int AttackPower => soldiers.Sum(p => p.Count * p.Type.AttackPower);
		public int DefensePower => soldiers.Sum(p => p.Count * p.Type.DefensePower);
		public int Weight => soldiers.Sum(p => p.Count * p.Type.Weight);
		public int Price => soldiers.Sum(p => p.Count * p.Type.Price);
		public int Count => soldiers.Sum(p => p.Count);
		public bool Any => soldiers.Any(p => p.Count > 0);

		public int TypeCount => soldiers.Length;
		public (SoldierType Type, int Count) this[int index] => soldiers[index];

		public bool CanMove(IProvinces provinces, int from, int to)
		{
			return Any && provinces[from].Soldiers.Contains(this) && soldiers.Sum(p => p.Count * (p.Type.CanMove(provinces, from, to) - p.Type.Weight)) >= 0;
		}
		public bool CanSurviveIn(Province province) => soldiers.Sum(p => (p.Type.CanSustain(province) - p.Type.Weight) * p.Count) >= 0;
		static int[] Fight(ImmutableArray<(SoldierType Type, int Count)> soldiers, int me, int enemy, Func<SoldierType, int> powerof)
		{
			int died = 0;
			int[] remaining = new int[soldiers.Length];
			for (int i = 0; i < remaining.Length; ++i)
			{
				if (powerof(soldiers[i].Type) > 0)
				{
					remaining[i] = soldiers[i].Count - (soldiers[i].Count * enemy / me);
					died += (soldiers[i].Count - remaining[i]) * powerof(soldiers[i].Type);
				}
				else
				{
					remaining[i] = soldiers[i].Count;
				}
			}
			for (int i = 0; died < enemy && i < remaining.Length; ++i)
			{
				if (powerof(soldiers[i].Type) > 0)
				{
					var d = Math.Min(remaining[i], ((enemy - died) / powerof(soldiers[i].Type)) + 1);
					remaining[i] -= d;
					died += d * powerof(soldiers[i].Type);
				}

			}
			return remaining;
		}
		public Soldiers AttackedBy(Soldiers s2)
		{
			int defensePower = DefensePower, attackPower = s2.AttackPower;
			var (s, power1, power2, powerof) = defensePower >= attackPower
					? (soldiers, defensePower, attackPower, (Func<SoldierType, int>)(type => type.DefensePower))
					: (s2.soldiers, attackPower, defensePower, type => type.AttackPower);
			var remaining = Fight(s, power1, power2, powerof).Select((count, i) => (s[i].Type, count));
			return new Soldiers(remaining.Where(pair => pair.count > 0));
		}
		public IEnumerator<(SoldierType, int)> GetEnumerator() => (soldiers as IEnumerable<(SoldierType, int)>).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
