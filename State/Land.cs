namespace Imperit.State
{
    public class Land : Province
    {
        public readonly bool IsStart;
        public Land(int id, string name, Shape shape, Army army, Army defaultArmy, bool isStart, int earnings, Settings settings)
            : base(id, name, shape, army, defaultArmy, earnings, settings) => IsStart = isStart;
        protected override Province WithArmy(Army army) => new Land(Id, Name, Shape, army, DefaultArmy, IsStart, Earnings, settings);
        public override Color Fill => Army.Color.Over(settings.LandColor);
        public int Price => Soldiers.Price + (Earnings * 2);
        public double Instability => settings.Instability(Soldiers, DefaultArmy.Soldiers);
    }
}