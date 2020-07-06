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
        public static T MinBy<T, C>(this IEnumerable<T> e, Func<T, C> selector) => e.OrderBy(selector).First();
        public static IEnumerable<T> MakeCopy<T>(this IEnumerable<T> e) => e.Select(x => x);
        public static T Must<T>(this T? value) where T : struct => value ?? throw new ArgumentNullException();
        public static double Pow(this double b, double e) => Math.Pow(b, e);
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
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
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
            static byte clamp(int i)
            {
                if (i < 0) return 0;
                if (i > 255) return 255;
                return (byte)i;
            }
        }
        public static State.Color NextColor(this Random rand)
        {
            var value = rand.Next(1,3);
            return HsvToRgb(rand.NextDouble()*360, rand.Next(value - 1, 3)/2.0, value/2.0);
        }
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