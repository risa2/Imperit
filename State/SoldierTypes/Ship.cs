using System;

namespace Imperit.State.SoldierTypes
{
	public class Ship : SoldierType
	{
		public override Description Description { get; }
		public override uint AttackPower { get; }
		public override uint DefensePower { get; }
		public override uint Weight { get; }
		public override uint Price { get; }
		public uint Capacity { get; }
		public Ship(int id, Description description, uint attackPower, uint defensePower, uint weight, uint price, uint capacity) : base(id)
		{
			Description = description;
			AttackPower = attackPower;
			DefensePower = defensePower;
			Weight = weight;
			Price = price;
			Capacity = capacity;
		}
		public override uint CanMove(IProvinces provinces, int from, int to)
		{
			return ((provinces[from] is Port && provinces[to] is Sea)
				  || (provinces[from] is Sea && provinces[to] is Sea)
				  || (provinces[from] is Sea && provinces[to] is Port)
				  || from == to) && provinces.Passable(from, to) ? Capacity + Weight : 0;
		}
		protected override IComparable Identity => (base.Identity, Capacity);
		public override bool IsRecruitable(Province province) => province is Port;
	}
}
