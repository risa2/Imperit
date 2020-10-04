using System.Collections;
using System.Collections.Generic;

namespace Imperit.State
{
	public abstract class Province : IAppearance
	{
		public readonly int Id;
		public readonly string Name;
		public readonly Shape Shape;
		public readonly Army Army, DefaultArmy;
		public readonly int Earnings;
		protected readonly Settings settings;
		public Province(int id, string name, Shape shape, Army army, Army defaultArmy, int earnings, Settings set)
		{
			Id = id;
			Name = name;
			Shape = shape;
			Army = army;
			DefaultArmy = defaultArmy;
			Earnings = earnings;
			settings = set;
		}
		public abstract Province GiveUpTo(Army army);
		public Province GiveUpTo(Player p) => GiveUpTo(new Army(new Soldiers(), p));
		public Province Revolt() => GiveUpTo(DefaultArmy);
		public virtual Province StartMove(Province dest, Soldiers army) => GiveUpTo(Army.Subtract(army));
		public Province AttackedBy(Army another) => GiveUpTo(Army.AttackedBy(another));
		public Province ReinforcedBy(Soldiers another) => GiveUpTo(Army.Join(another));
		public bool IsAllyOf(int p) => Army.IsAllyOf(p);
		public bool IsAllyOf(Army army) => Army.IsAllyOf(army);
		public bool IsAllyOf(Province prov) => IsAllyOf(prov.Army);
		public Soldiers Soldiers => Army.Soldiers;
		public Soldiers DefaultSoldiers => DefaultArmy.Soldiers;
		public IEnumerable<SoldierType> SoldierTypes => Soldiers.Types;
		public bool Occupied => !Army.IsControlledBySavage;
		public bool CanSoldiersSurvive => Soldiers.CanSurviveIn(this);
		public virtual Color Fill => new Color();
		public virtual Color Stroke => new Color();
		public virtual int StrokeWidth => 0;
		public IEnumerator<Point> GetEnumerator() => Shape.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public Point Center => Shape.Center;

		public override bool Equals(object? obj) => obj != null && obj is Province p && p.Id == Id;
		public override int GetHashCode() => Id.GetHashCode();
		public static bool operator ==(Province? a, Province? b) => (a is null && b is null) || (a is object x && x.Equals(b));
		public static bool operator !=(Province? a, Province? b) => ((a is null) != (b is null)) || (a is object x && !x.Equals(b));
	}
}