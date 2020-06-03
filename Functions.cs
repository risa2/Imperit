using System;
using System.Linq;
using System.Collections.Generic;

namespace Imperit
{
    public static class Extension
    {
        public static IEnumerable<R> Casted<R, T>(this IEnumerable<T> e) => e.SelectMany(x => x is R r ? new R[] { r } : new R[0]);
        public static T FirstOr<T>(this IEnumerable<T> e, Func<T, bool> cond, T x) => e.Where(cond).DefaultIfEmpty(x).First();
        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> e) => e.Select((v, i) => (i, v));
        public static IEnumerable<int> Range(this int e) => Enumerable.Range(0, e);
        public static T MinBy<T, C>(this IEnumerable<T> e, Func<T, C> selector) => e.OrderBy(selector).First();
        public static IEnumerable<T> MakeCopy<T>(this IEnumerable<T> e) => e.Select(x => x);
    }
    public interface IArray<T> : IReadOnlyList<T>, IList<T>
    {
        new int Count { get; }
        int ICollection<T>.Count => this.Count;
        int IReadOnlyCollection<T>.Count => this.Count;
        new T this[int i] { get; set; }
        T IList<T>.this[int i]
        {
            get => this[i];
            set => this[i] = value;
        }
        T IReadOnlyList<T>.this[int i] => this[i];
    }
}