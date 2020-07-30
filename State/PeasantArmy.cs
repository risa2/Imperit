namespace Imperit.State
{
    public class PeasantArmy : IArmy
    {
        public uint Soldiers { get; }
        public Color Color => new Color();
        public PeasantArmy(uint soldiers) => Soldiers = soldiers;
        public Dynamics.IAction? Gain(Province where) => null;
        public Dynamics.IAction? Lose(Province where) => null;
        public IArmy Join(IArmy another) => new PeasantArmy(Soldiers + another.Soldiers);
        public IArmy Subtract(IArmy another) => new PeasantArmy(Soldiers - another.Soldiers);
        public bool IsAllyOf(IArmy another) => false;
        public bool IsControlledBy(int player) => false;
    }
}