using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.State
{
	public class Soldiers : IEnumerable<(SoldierType Type, uint Count)>
	{
		readonly ImmutableArray<(SoldierType Type, uint Count)> soldiers;
		public Soldiers() => soldiers = ImmutableArray<(SoldierType Type, uint Count)>.Empty;
		public Soldiers(ImmutableArray<(SoldierType, uint)> list) => soldiers = list;
		public Soldiers(IEnumerable<(SoldierType, uint)> list) => soldiers = list.ToImmutableArray();
		public Soldiers(params (SoldierType, uint)[] list) => soldiers = list.ToImmutableArray();
		public Soldiers Add(Soldiers snd)
		{
			var bld = soldiers.ToBuilder();
			bld.InsertMatch(snd.soldiers, (a, b) => a.Type == b.Type, (a, b) => (a.Type, a.Count + b.Count));
			return new Soldiers(bld.ToImmutable());
		}
		public bool Contains(Soldiers snd)
		{
			return snd.soldiers.All(s2 => soldiers.Any(s1 => s1.Type == s2.Type && s1.Count >= s2.Count));
		}
		public Soldiers Subtract(Soldiers snd)
		{
			var bld = soldiers.ToBuilder();
			bld.InsertMatch(snd.soldiers, (a, b) => a.Type == b.Type, (a, b) => (a.Type, a.Count - b.Count));
			return new Soldiers(bld.Where(x => x.Count > 0).ToImmutableArray());
		}
		public Soldiers Multiply(uint mul) => new Soldiers(soldiers.Select(s => (s.Type, s.Count * mul)));
		public IEnumerable<(SoldierType Type, uint Count)> Split() => soldiers;
		public IEnumerable<SoldierType> Types => soldiers.Select(p => p.Type);
		public uint AttackPower => (uint)soldiers.Sum(p => p.Count * p.Type.AttackPower);
		public uint DefensePower => (uint)soldiers.Sum(p => p.Count * p.Type.DefensePower);
		public uint Weight => (uint)soldiers.Sum(p => p.Count * p.Type.Weight);
		public uint Price => (uint)soldiers.Sum(p => p.Count * p.Type.Price);
		public uint Count => (uint)soldiers.Sum(p => p.Count);
		public bool Any => soldiers.Any(p => p.Count > 0);

		public int TypeCount => soldiers.Length;
		public (SoldierType Type, uint Count) this[int index] => soldiers[index];

		public bool CanMove(IProvinces provinces, int from, int to)
		{
			return Any && provinces[from].Soldiers.Contains(this) && soldiers.Sum(p => p.Count * (p.Type.CanMove(provinces, from, to) - p.Type.Weight)) >= 0;
		}
		static uint[] Fight(ImmutableArray<(SoldierType Type, uint Count)> soldiers, uint me, uint enemy, Func<SoldierType, uint> powerof)
		{
			uint died = 0;
			uint[] remaining = new uint[soldiers.Length];
			for (int i = 0; i < remaining.Length; ++i)
			{
				remaining[i] = soldiers[i].Count - (soldiers[i].Count * enemy / me);
				died += (soldiers[i].Count - remaining[i]) * powerof(soldiers[i].Type);
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
			uint defensePower = DefensePower, attackPower = s2.AttackPower;
			var (s, power1, power2, powerof) = defensePower >= attackPower
					? (soldiers, defensePower, attackPower, (Func<SoldierType, uint>)(type => type.DefensePower))
					: (s2.soldiers, attackPower, defensePower, type => type.AttackPower);
			var remaining = Fight(s, power1, power2, powerof).Select((count, i) => (s[i].Type, count));
			return new Soldiers(remaining.Where(pair => pair.count > 0));
		}
		public IEnumerator<(SoldierType, uint)> GetEnumerator() => (soldiers as IEnumerable<(SoldierType, uint)>).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
