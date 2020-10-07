using System;
using System.Collections.Immutable;

namespace Imperit.State.SoldierTypes
{
	public class ElephantShip : SoldierType
	{
		public override Description Description { get; }
		public override int AttackPower { get; }
		public override int DefensePower { get; }
		public override int Weight { get; }
		public override int Price { get; }
		public int Capacity { get; }
		public int Speed { get; }
		public ImmutableArray<int> RecruitPlaces { get; }
		public ElephantShip(int id, Description description, int attackPower, int defensePower, int weight, int price, int capacity, int speed, ImmutableArray<int> recruitPlaces) : base(id)
		{
			Description = description;
			AttackPower = attackPower;
			DefensePower = defensePower;
			Weight = weight;
			Price = price;
			Capacity = capacity;
			Speed = speed;
			RecruitPlaces = recruitPlaces;
		}
		protected override IComparable Identity => (base.Identity, Capacity, Speed, RecruitPlaces);
		int Difficulty(Province to) => Ship.IsPassable(to) ? 1 : Speed + 1;
		public override int CanMove(IProvinces provinces, int from, int dest)
		{
			return Ship.IsPassable(provinces[from]) && Ship.IsPassable(provinces[dest])
				&& provinces.Passable(from, dest, Speed, (_, to) => Difficulty(to)) ? Capacity + Weight : 0;
		}
		public override int CanSustain(Province province) => province is Sea ? Capacity + Weight : province is Port ? Weight : 0;
		public override bool IsRecruitable(Province province) => RecruitPlaces.Contains(province.Id);
	}
}
