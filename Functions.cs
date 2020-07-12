using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit
{
    public static class Extension
    {
        public static IEnumerable<R> Casted<R, T>(this IEnumerable<T> e) => e.SelectMany(x => x is R r ? new R[] { r } : new R[0]);
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> e) where T : class => e.Where(it => it != null)!;
        public static T FirstOr<T>(this IEnumerable<T> e, Func<T, bool> cond, T x) => e.Where(cond).DefaultIfEmpty(x).First();
        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> e) => e.Select((v, i) => (i, v));
        public static T MinBy<T, C>(this IEnumerable<T> e, Func<T, C> selector) => e.OrderBy(selector).First();
        public static IEnumerable<T> MakeCopy<T>(this IEnumerable<T> e) => e.Select(x => x);
        public static T Must<T>(this T? value) where T : struct => value ?? throw new ArgumentNullException();
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> e) => e.SelectMany(x => x);
        public static State.Color HsvToRgb(double H, double S, double V)
        {
            while (H < 0) { H += 360; }
            while (H >= 360) { H -= 360; }
            double R, G, B;
            if (V <= 0)
            {
                R = G = B = 0;
            }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - (S * f));
                double tv = V * (1 - (S * (1 - f)));
                switch (i)
                {
                    // Red is the dominant color
                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;
                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    // Green is the dominant color
                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;
                    // Blue is the dominant color
                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;
                    // Just in case we overshoot on our math by a little, we put these here.
                    // Since its a switch it won't slow us down at all to put these here
                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;
                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            return new State.Color(clamp((int)(R * 255.0)), clamp((int)(G * 255.0)), clamp((int)(B * 255.0)));
            static byte clamp(int i) => i < 0 ? (byte)0 : i > 255 ? (byte)255 : (byte)i;
        }
        public static State.Color NextColor(this Random rand)
        {
            return HsvToRgb(rand.Next(360), (rand.Next(2) + 1) / 2.0, (rand.Next(2) + 1) / 2.0);
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
        public static T[] SortBy<T>(this IReadOnlyList<T> list, Func<T, byte> value)
        {
            T[] result = new T[list.Count];
            uint[] counts = new uint[256];
            uint[] starts = new uint[256];
            for (int i = 0; i < list.Count; ++i)
            {
                ++counts[value(list[i])];
            }
            for (int i = 1; i < 256; ++i)
            {
                starts[i] = starts[i - 1] + counts[i - 1];
            }
            for (int i = 0; i < list.Count; ++i)
            {
                byte idx = value(list[i]);
                result[starts[idx]] = list[i];
                ++starts[idx];
            }
            return result;
        }
        public static (U, T) Swap<T, U>(this (T, U) pair) => (pair.Item2, pair.Item1);
        public static (A, B) Unzip<T, U, A, B>(this IEnumerable<(T, U)> en, Func<IEnumerable<T>, A> s1, Func<IEnumerable<U>, B> s2)
        {
            return (s1(en.Select(it => it.Item1)), s2(en.Select(it => it.Item2)));
        }
        public static (IEnumerable<T>, IEnumerable<U>) Unzip<T, U>(this IEnumerable<(T, U)> en) => Unzip(en, x => x, x => x);
        public static (V[], A) MapFold<T, U, V, A>(this IReadOnlyList<T> lst, A init, Func<T, U> selector, Func<U, V> selector2, Func<A, U, A> acc)
        {
            var result = new V[lst.Count];
            for (int i = 0; i < lst.Count; ++i)
            {
                var x = selector(lst[i]);
                result[i] = selector2(x);
                init = acc(init, x);
            }
            return (result, init);
        }
    }
    public interface IArray<T> : IReadOnlyList<T>
    {
        new T this[int i] { get; set; }
        T IReadOnlyList<T>.this[int i] => this[i];
    }
}