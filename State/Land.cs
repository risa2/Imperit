namespace Imperit.State
{
    public class Land : Province
    {
        public readonly bool IsStart;
        public Land(int id, string name, IArmy army, IArmy defaultArmy, bool isStart, uint earnings)
            : base(id, name, army, defaultArmy, earnings) => IsStart = isStart;
        protected override Province WithArmy(IArmy army) => new Land(Id, Name, army, DefaultArmy, IsStart, Earnings);
        public override uint CanMoveTo(Province dest) => dest is Sea ? 0 : Army.Soldiers;
        public uint Price => Army.Soldiers + (Earnings * 2);
        public double Instability => Army.Hostility;
    }
}