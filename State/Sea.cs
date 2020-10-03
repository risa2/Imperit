namespace Imperit.State
{
	public class Sea : Province
	{
		public Sea(int id, string name, Shape shape, Army army, Army defaultArmy, Settings settings)
			: base(id, name, shape, army, defaultArmy, 0, settings) { }
		public override Province GiveUpTo(Army army) => new Sea(Id, Name, Shape, army, DefaultArmy, settings);
		public override Color Fill => Army.Color.Mix(settings.SeaColor);
	}
}