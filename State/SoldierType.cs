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
		public abstract uint AttackPower { get; }
		public abstract uint DefensePower { get; }
		public abstract uint Weight { get; }
		public abstract uint Price { get; }
		public abstract bool IsRecruitable(Province province);
		public abstract uint CanMove(IProvinces provinces, int from, int to);
		protected virtual IComparable Identity => (GetType(), Name, Symbol, Text, AttackPower, DefensePower, Weight, Price);
		public int CompareTo(SoldierType? type) => Identity.CompareTo(type?.Identity);
		public sealed override int GetHashCode() => Identity.GetHashCode();
		public virtual bool Equals(SoldierType? t) => CompareTo(t) == 0;
		public sealed override bool Equals(object? obj) => Equals(obj as SoldierType);
	}
}
