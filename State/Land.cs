namespace Imperit.State
{
    public class Land : Province
    {
        public readonly bool IsStart;
        public Land(int id, string name, Shape shape, IArmy army, IArmy defaultArmy, bool isStart, uint earnings, Settings settings)
            : base(id, name, shape, army, defaultArmy, earnings, settings) => IsStart = isStart;
        protected override Province WithArmy(IArmy army) => new Land(Id, Name, Shape, army, DefaultArmy, IsStart, Earnings, settings);
        public override uint CanMoveTo(Province dest) => dest is Sea ? 0 : Army.Soldiers;
        public override Color Fill => base.Fill.Over(settings.LandColor);
        public uint Price => Army.Soldiers + (Earnings * 2);
        public double Instability => settings.Instability(Army.Soldiers, DefaultArmy.Soldiers);
    }
}