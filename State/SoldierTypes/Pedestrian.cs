namespace Imperit.State.SoldierTypes
{
	public class Pedestrian : SoldierType
	{
		public override Description Description { get; }
		public override int AttackPower { get; }
		public override int DefensePower { get; }
		public override int Weight { get; }
		public override int Price { get; }
		public Pedestrian(int id, Description description, int attackPower, int defensePower, int weight, int price) : base(id)
		{
			Description = description;
			AttackPower = attackPower;
			DefensePower = defensePower;
			Weight = weight;
			Price = price;
		}
		public override int CanMove(IProvinces provinces, int from, int to)
		{
			return provinces[from] is Land && provinces[to] is Land && provinces.Passable(from, to) ? Weight : 0;
		}
		public override bool IsRecruitable(Province province) => province is Land;
		public override int CanSustain(Province province) => province is Land ? Weight : 0;
	}
}
