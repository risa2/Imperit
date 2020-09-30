namespace Imperit.State.SoldierTypes
{
	public class Pedestrian : SoldierType
	{
		public override Description Description { get; }
		public override uint AttackPower { get; }
		public override uint DefensePower { get; }
		public override uint Weight { get; }
		public override uint Price { get; }
		public Pedestrian(int id, Description description, uint attackPower, uint defensePower, uint weight, uint price) : base(id)
		{
			Description = description;
			AttackPower = attackPower;
			DefensePower = defensePower;
			Weight = weight;
			Price = price;
		}
		public override uint CanMove(IProvinces provinces, int from, int to)
		{
			return provinces[from] is Land && provinces[to] is Land && provinces.Passable(from, to) ? Weight : 0;
		}
		public override bool IsRecruitable(Province province) => province is Land;
	}
}
