using System;

namespace Imperit.State.SoldierTypes
{
	public class Ship : SoldierType
	{
		public override Description Description { get; }
		public override int AttackPower { get; }
		public override int DefensePower { get; }
		public override int Weight { get; }
		public override int Price { get; }
		public int Capacity { get; }
		public Ship(int id, Description description, int attackPower, int defensePower, int weight, int price, int capacity) : base(id)
		{
			Description = description;
			AttackPower = attackPower;
			DefensePower = defensePower;
			Weight = weight;
			Price = price;
			Capacity = capacity;
		}
		protected override IComparable Identity => (base.Identity, Capacity);
		public override int CanMove(IProvinces provinces, int from, int to)
		{
			return ((provinces[from] is Port && provinces[to] is Sea)
				  || (provinces[from] is Sea && provinces[to] is Sea)
				  || (provinces[from] is Sea && provinces[to] is Port))
				  && provinces.Passable(from, to) ? Capacity + Weight : 0;
		}
		public override bool IsRecruitable(Province province) => province is Port;
		public override int CanSustain(Province province) => province is Sea ? Capacity + Weight : province is Port ? Weight : 0;
	}
}
