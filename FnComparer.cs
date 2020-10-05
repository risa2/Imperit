using System;
using System.Collections.Generic;

namespace Imperit
{
	public class FnComparer<T, TV> : IComparer<T>, IEqualityComparer<T> where TV : IComparable<TV>, IEquatable<TV> where T : class
	{
		readonly Func<T, TV> getValue;
		public readonly bool AllowDuplicates;
		public FnComparer(Func<T, TV> key, bool allowDuplicates = false)
		{
			getValue = key;
			AllowDuplicates = allowDuplicates;
		}
		public int Compare(T? x, T? y)
		{
			if (x is null)
			{
				return y is null ? 0 : -1;
			}
			if (y is null)
			{
				return 1;
			}
			int compared = getValue(x).CompareTo(getValue(y));
			if (!AllowDuplicates || compared != 0)
			{
				return compared;
			}
			int hashCodeCompare = x?.GetHashCode().CompareTo(y?.GetHashCode()) ?? 0;
			return hashCodeCompare != 0 ? hashCodeCompare : ReferenceEquals(x, y) ? 0 : -1;
		}
		public bool Equals(T? x, T? y)
		{
			return !AllowDuplicates && ((x is null && y is null) || (x is T && y is T && getValue(x).Equals(getValue(y))));
		}
		public int GetHashCode(T obj) => getValue(obj).GetHashCode();
	}
}
