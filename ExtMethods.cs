using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Imperit
{
	public static class ExtMethods
	{
		public static CultureInfo Culture { get; } = CultureInfo.InvariantCulture;
		public static T FirstOr<T>(this IEnumerable<T> e, T x) => e.DefaultIfEmpty(x).First();
		public static IEnumerable<(int i, T v)> Index<T>(this IEnumerable<T> e) => e.Select((v, i) => (i, v));
		public static T? MinBy<T, TC>(this IEnumerable<T> e, Func<T, TC> selector, T? v = default) where T : class => e.OrderBy(selector).FirstOr(v);
		public static T Must<T>(this T? value) where T : struct => value ?? throw new ArgumentNullException();
		public static IEnumerable<T> Must<T>(this IEnumerable<T?> e) where T : class => e.Where(x => x is T)!;
		public static int DivUp(this int a, int b) => (a / b) + (a % b > 0 ? 1 : 0);
		public static IEnumerable<T> Infinity<T>(this T value)
		{
			while (true)
			{
				yield return value;
			}
		}
		public static int Find<T>(this IList<T> ts, Func<T, bool> cond)
		{
			int i = 0;
			while (i < ts.Count && !cond(ts[i]))
			{
				++i;
			}
			return i;
		}
		public static void InsertMatch<T>(this IList<T> s1, IEnumerable<T> s2, Func<T, T, bool> eq, Func<T, T, T> match)
		{
			foreach (var t2 in s2)
			{
				var i = s1.Find(t1 => eq(t1, t2));
				if (i < s1.Count)
				{
					s1[i] = match(s1[i], t2);
				}
				else
				{
					s1.Add(t2);
				}
			}
		}
		public static string ProbabilityToString(this double prob, int prec = 0)
		{
			int percents = (int)(prob * 100);
			int frac = percents >= 100 ? 0 : (int)(prob * 100 * Math.Pow(10, prec)) - (int)(percents * Math.Pow(10, prec));
			return Math.Max(0, Math.Min(100, percents)).ToString(Culture) + "." + frac.ToString(Culture).PadRight(prec, '0') + " %";
		}
		public static string ToHexString(this byte num) => num.ToString("x2", Culture);
		public static string NextId(this Random rand, int length)
		{
			var buf = new byte[length];
			rand.NextBytes(buf);
			return Convert.ToBase64String(buf).TrimEnd('=').Replace('+', '-').Replace('/', '_');
		}
		public static ImmutableSortedSet<T> AddRange<T>(this ImmutableSortedSet<T> set, IEnumerable<T> items)
		{
			ImmutableSortedSet<T>.Builder builder = set.ToBuilder();
			builder.UnionWith(items);
			return builder.ToImmutable();
		}
		public static void Shuffle<T>(this Random rand, IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rand.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
		public static (TA, TB) Unzip<T, TU, TA, TB>(this IEnumerable<(T, TU)> en, Func<IEnumerable<T>, TA> s1, Func<IEnumerable<TU>, TB> s2)
		{
			return (s1(en.Select(it => it.Item1)), s2(en.Select(it => it.Item2)));
		}
		public static (IEnumerable<T>, IEnumerable<TU>) Unzip<T, TU>(this IEnumerable<(T, TU)> en) => Unzip(en, x => x, x => x);
	}
}