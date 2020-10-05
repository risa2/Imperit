using System;
using System.Globalization;

namespace Imperit.State
{
	public readonly struct Color : IEquatable<Color>
	{
		static byte FromHex(string s) => byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		public readonly byte r, g, b, a;
		public Color(byte R, byte G, byte B, byte A = 255) => (r, g, b, a) = (R, G, B, A);
		public override string ToString() => "#" + r.ToHexString() + g.ToHexString() + b.ToHexString() + a.ToHexString();
		public static Color Parse(string str) => new Color(FromHex(str[1..3]), FromHex(str[3..5]), FromHex(str[5..7]), str.Length < 9 ? (byte)255 : FromHex(str[7..9]));
		static byte Mix(byte a, byte b, int w1, int w2) => (byte)(((a * w1) + (b * w2)) / (w1 + w2));
		static byte Supl(byte x, byte y) => (byte)(255 - ((255 - x) * (255 - y) / 255));
		public Color Mix(Color color) => new Color(Mix(r, color.r, a, color.a), Mix(g, color.g, a, color.a), Mix(b, color.b, a, color.a), Supl(a, color.a));
		public Color Over(Color color) => new Color(Mix(r, color.r, 255, 255 - a), Mix(g, color.g, 255, 255 - a), Mix(b, color.b, 255, 255 - a), Supl(a, color.a));
		public bool Equals(Color c2) => (r, g, b, a) == (c2.r, c2.g, c2.b, c2.a);
		public override bool Equals(object? obj) => obj is Color col && Equals(col);
		public override int GetHashCode() => (r, g, b, a).GetHashCode();
		public static bool operator ==(Color left, Color right) => left.Equals(right);
		public static bool operator !=(Color left, Color right) => !left.Equals(right);
		public Color WithAlpha(byte alpha) => new Color(r, g, b, alpha);
		public Color(double H, double S, double V)
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
					case -1:
					case 5:
						R = V;
						G = pv;
						B = qv;
						break;
					case 6:
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
					// The color is not defined, we should throw an error
					default:
						R = G = B = V; // Just pretend its black/white
						break;
				}
			}
			static byte clamp(int i) => i < 0 ? (byte)0 : i > 255 ? (byte)255 : (byte)i;
			r = clamp((int)(R * 255.0));
			g = clamp((int)(G * 255.0));
			b = clamp((int)(B * 255.0));
			a = 255;
		}
	}
}