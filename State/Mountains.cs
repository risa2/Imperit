namespace Imperit.State
{
    public class Mountains : Province
    {
        public Mountains(int id, string name, Shape shape, IArmy army, IArmy defaultArmy, Settings set)
            : base(id, name, shape, army, defaultArmy, 0, set) { }
        public override Color Stroke => settings.MountainsColor;
        public override int StrokeWidth => settings.MountainsWidth;
        public override uint CanMoveTo(Province dest) => 0;
        protected override Province WithArmy(IArmy army) => new Mountains(Id, Name, Shape, Army, DefaultArmy, settings);
    }
}