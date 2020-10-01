using System;

namespace Imperit.State
{
	public abstract class SoldierType : IEquatable<SoldierType>, IComparable<SoldierType>
	{
		public readonly int Id;
		protected SoldierType(int id) => Id = id;
		public abstract Description Description { get; }
		public string Name => Description.Name;
		public string Symbol => Description.Symbol;
		public string Text => Description.Text;
		public abstract int AttackPower { get; }
		public abstract int DefensePower { get; }
		public abstract int Weight { get; }
		public abstract int Price { get; }
		public abstract bool IsRecruitable(Province province);
		public abstract int CanSustain(Province province);
		public abstract int CanMove(IProvinces provinces, int from, int to);
		public bool CanMoveAlone(IProvinces provinces, int from, int to) => CanMove(provinces, from, to) >= Weight;
		protected virtual IComparable Identity => (GetType(), Name, Symbol, Text, AttackPower, DefensePower, Weight, Price);
		public int CompareTo(SoldierType? type) => Identity.CompareTo(type?.Identity);
		public sealed override int GetHashCode() => Identity.GetHashCode();
		public virtual bool Equals(SoldierType? t) => CompareTo(t) == 0;
		public sealed override bool Equals(object? obj) => Equals(obj as SoldierType);
	}
}
