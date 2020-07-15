using System;
using System.Collections.Generic;

namespace Imperit
{
    public class AllowDuplicatesComparer<T, TV> : IComparer<T> where TV : IComparable<TV>
    {
        readonly Func<T, TV> getValue;
        public AllowDuplicatesComparer(Func<T, TV> key)
        {
            getValue = key;
        }
#nullable disable
        public int Compare(T x, T y)
#nullable enable
        {
            int compared = getValue(x).CompareTo(getValue(y));
            if (compared != 0)
            {
                return compared;
            }
            int hashCodeCompare = x?.GetHashCode().CompareTo(y?.GetHashCode()) ?? 0;
            return hashCodeCompare != 0 ? hashCodeCompare : ReferenceEquals(x, y) ? 0 : -1;
        }
    }
}
