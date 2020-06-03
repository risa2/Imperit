namespace Imperit.State
{
    public abstract class Province
    {
        public readonly int Id;
        public readonly string Name;
        public readonly IArmy Army, DefaultArmy;
        public readonly uint Earnings;
        public Province(int id, string name, IArmy army, IArmy defaultArmy, uint earnings)
        {
            Id = id;
            Name = name;
            Army = army;
            DefaultArmy = defaultArmy;
            Earnings = earnings;
        }
        public abstract uint CanMoveTo(Province dest);
        protected abstract Province WithArmy(IArmy army);
        public (Province, Dynamics.IAction) GiveUpTo(IArmy his_army)
        {
            return (WithArmy(his_army), new Dynamics.Actions.Combination(Army.Lose(this), his_army.Gain(this)));
        }
        public (Province, Dynamics.IAction) Revolt() => GiveUpTo(DefaultArmy);
        public virtual Province StartMove(Province dest, IArmy army) => WithArmy(Army.Subtract(army));
        public (Province, Dynamics.IAction) AttackedBy(IArmy another) => GiveUpTo(Army.AttackedBy(another));
        public Province ReinforcedBy(IArmy another) => WithArmy(Army.Join(another));
        public bool IsControlledBy(Player p) => Army.IsControlledBy(p);
        public bool IsAllyOf(IArmy army) => Army.IsAllyOf(army);
        public uint Soldiers => Army.Soldiers;
        public bool Occupied => !(Army is PeasantArmy);

        public override bool Equals(object? obj) => obj != null && obj is Province p && p.Id == Id;
        public override int GetHashCode() => Id.GetHashCode();
        public static bool operator ==(Province? a, Province? b) => ((object?)a == null && (object?)b == null) || ((object?)a != null && a.Equals(b));
        public static bool operator !=(Province? a, Province? b) => (((object?)a == null) != ((object?)b == null)) || ((object?)a != null && !a.Equals(b));
    }
}