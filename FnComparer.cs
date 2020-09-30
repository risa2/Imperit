using System;
using System.Collections.Generic;

namespace Imperit
{
    public class FnComparer<T, TV> : IComparer<T>, IEqualityComparer<T> where TV : IComparable<TV>, IEquatable<TV>
    {
        readonly Func<T, TV> getValue;
        public readonly bool AllowDuplicates;
        public FnComparer(Func<T, TV> key, bool allowDuplicates = false)
        {
            getValue = key;
            AllowDuplicates = allowDuplicates;
        }
#nullable disable
        public int Compare(T x, T y)
        {
            int compared = getValue(x).CompareTo(getValue(y));
            if (!AllowDuplicates || compared != 0)
            {
                return compared;
            }
            int hashCodeCompare = x?.GetHashCode().CompareTo(y?.GetHashCode()) ?? 0;
            return hashCodeCompare != 0 ? hashCodeCompare : ReferenceEquals(x, y) ? 0 : -1;
        }
        public bool Equals(T x, T y) => !AllowDuplicates && getValue(x).Equals(getValue(y));
#nullable enable
        public int GetHashCode(T obj) => getValue(obj).GetHashCode();
    }
}
