namespace Imperit.State
{
	public class Mountains : Province
	{
		public Mountains(int id, string name, Shape shape, Army army, Army defaultArmy, Settings set)
			: base(id, name, shape, army, defaultArmy, 0, set) { }
		public override Color Stroke => settings.MountainsColor;
		public override int StrokeWidth => settings.MountainsWidth;
		public override Province GiveUpTo(Army army) => new Mountains(Id, Name, Shape, Army, DefaultArmy, settings);
	}
}