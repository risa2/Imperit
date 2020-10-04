using System.Collections.Immutable;

namespace Imperit.State.SoldierTypes
{
	public class Elephant : SoldierType
	{
		public override Description Description { get; }
		public override int AttackPower { get; }
		public override int DefensePower { get; }
		public override int Weight { get; }
		public override int Price { get; }
		public int Capacity { get; }
		public int Speed { get; }
		public ImmutableArray<int> RecruitPlaces { get; }
		public Elephant(int id, Description description, int attackPower, int defensePower, int weight, int price, int capacity, int speed, ImmutableArray<int> recruitPlaces) : base(id)
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
		public override int CanMove(IProvinces provinces, int from, int to)
		{
			return provinces.Distance(from, to) <= Speed && provinces[from] is Land && provinces[to] is Land ? Weight + Capacity : 0;
		}
		public override int CanSustain(Province province) => province is Land ? Capacity + Weight : Weight;
		public override bool IsRecruitable(Province province) => RecruitPlaces.Contains(province.Id);
	}
}
