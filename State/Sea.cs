namespace Imperit.State
{
    public class Sea : Province
    {
        public Sea(int id, string name, Shape shape, IArmy army, Settings settings)
            : base(id, name, shape, army, new PeasantArmy(0), 0, settings) { }
        protected override Province WithArmy(IArmy army) => new Sea(Id, Name, Shape, army, settings);
        public override uint CanMoveTo(Province dest) => dest is Sea || dest is Port ? Army.Soldiers : 0;
        public override Color Fill => Army.Color.Mix(settings.SeaColor);
    }
}