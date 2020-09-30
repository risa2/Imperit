using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
	public abstract class Province : IAppearance
	{
		public readonly int Id;
		public readonly string Name;
		public readonly Shape Shape;
		public readonly Army Army, DefaultArmy;
		public readonly uint Earnings;
		protected readonly Settings settings;
		public Province(int id, string name, Shape shape, Army army, Army defaultArmy, uint earnings, Settings set)
		{
			Id = id;
			Name = name;
			Shape = shape;
			Army = army;
			DefaultArmy = defaultArmy;
			Earnings = earnings;
			settings = set;
		}
		protected abstract Province WithArmy(Army army);
		public (Province, Dynamics.IAction[]) GiveUpTo(Army his_army)
		{
			return (WithArmy(his_army), new[] { Army.Lose(this), his_army.Gain(this) }.NotNull().ToArray());
		}
		public (Province, Dynamics.IAction[]) GiveUpTo(Player p) => GiveUpTo(new Army(new Soldiers(), p));
		public (Province, Dynamics.IAction[]) Revolt() => GiveUpTo(DefaultArmy);
		public virtual Province StartMove(Province dest, Army army) => WithArmy(Army.Subtract(army));
		public (Province, Dynamics.IAction[]) AttackedBy(Army another) => GiveUpTo(Army.AttackedBy(another));
		public Province ReinforcedBy(Army another) => WithArmy(Army.Join(another));
		public bool IsControlledBy(int p) => Army.IsControlledBy(p);
		public bool IsAllyOf(Army army) => Army.IsAllyOf(army);
		public bool IsAllyOf(Province prov) => Army.IsAllyOf(prov.Army);
		public Soldiers Soldiers => Army.Soldiers;
		public Soldiers DefaultSoldiers => DefaultArmy.Soldiers;
		public IEnumerable<SoldierType> SoldierTypes => Soldiers.Types;
		public bool Occupied => Army.PlayerType != typeof(Savage);
		public virtual Color Fill => new Color();
		public virtual Color Stroke => new Color();
		public virtual int StrokeWidth => 0;
		public IEnumerator<Point> GetEnumerator() => Shape.GetEnumerator();
		public Point Center => Shape.Center;

		public override bool Equals(object? obj) => obj != null && obj is Province p && p.Id == Id;
		public override int GetHashCode() => Id.GetHashCode();
		public static bool operator ==(Province? a, Province? b) => (a is null && b is null) || (a is object x && x.Equals(b));
		public static bool operator !=(Province? a, Province? b) => ((a is null) != (b is null)) || (a is object x && !x.Equals(b));
	}
}