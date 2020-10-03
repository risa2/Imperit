namespace Imperit.State
{
	public class Land : Province
	{
		public readonly bool IsStart;
		public Land(int id, string name, Shape shape, Army army, Army defaultArmy, bool isStart, int earnings, Settings settings)
			: base(id, name, shape, army, defaultArmy, earnings, settings) => IsStart = isStart;
		public override Province GiveUpTo(Army army) => new Land(Id, Name, Shape, army, DefaultArmy, IsStart, Earnings, settings);
		public int Price => (Earnings * 2) + Soldiers.Price;
		public override Color Fill => Army.Color.Over(settings.LandColor);
		public double Instability => settings.Instability(Soldiers, DefaultArmy.Soldiers);
	}
}