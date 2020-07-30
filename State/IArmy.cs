namespace Imperit.State
{
    public interface IArmy
    {
        uint Soldiers { get; }
        Color Color { get; }
        Dynamics.IAction? Lose(Province where);
        Dynamics.IAction? Gain(Province where);
        IArmy Join(IArmy another);
        IArmy Subtract(IArmy another);
        bool IsAllyOf(IArmy another);
        bool IsControlledBy(int player);
        IArmy AttackedBy(IArmy another) => another.Soldiers > Soldiers ? another.Subtract(this) : Subtract(another);
    }
}