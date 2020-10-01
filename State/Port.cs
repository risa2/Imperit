namespace Imperit.State
{
	public class Port : Land
	{
		public Port(int id, string name, Shape shape, Army army, Army defaultArmy, bool isStart, int earnings, Settings settings)
			: base(id, name, shape, army, defaultArmy, isStart, earnings, settings) { }
		protected override Province WithArmy(Army army) => new Port(Id, Name, Shape, army, DefaultArmy, IsStart, Earnings, settings);
	}
}