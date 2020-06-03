namespace Imperit.State
{
    public class PeasantArmy : IArmy
    {
        public uint Soldiers { get; }
        public PeasantArmy(uint soldiers) => Soldiers = soldiers;
        public double Hostility => 0.0;
        public Dynamics.IAction Gain(Province where) => new Dynamics.Actions.Nothing();
        public Dynamics.IAction Lose(Province where) => new Dynamics.Actions.Nothing();
        public IArmy Join(IArmy another) => new PeasantArmy(Soldiers + another.Soldiers);
        public IArmy Subtract(IArmy another) => new PeasantArmy(Soldiers - another.Soldiers);
        public bool IsAllyOf(IArmy another) => false;
        public bool IsControlledBy(Player player) => false;
    }
}