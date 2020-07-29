using System.Globalization;

namespace Imperit.State
{
    public readonly struct Color : System.IEquatable<Color>
    {
        static byte FromHex(string s) => byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        public readonly byte r, g, b;
        public Color(byte R, byte G, byte B)
        {
            r = R;
            g = G;
            b = B;
        }
        public static Color Black => new Color();
        public static Color White => new Color(255, 255, 255);
        public override string ToString() => "#" + r.ToHexString() + g.ToHexString() + b.ToHexString();
        public static Color Parse(string str) => new Color(FromHex(str[1..3]), FromHex(str[3..5]), FromHex(str[5..7]));
        static byte Mix(byte a, byte b, int w1, int w2) => (byte)(((a * w1) + (b * w2)) / (w1 + w2));
        public Color Mix(Color color, int w1 = 1, int w2 = 1) => new Color(Mix(r, color.r, w1, w2), Mix(g, color.g, w1, w2), Mix(b, color.b, w1, w2));
        public Color Darken(byte light) => Mix(new Color(), light, 255 - light);

        public bool Equals(Color other) => r == other.r && g == other.g && b == other.b;
        public override bool Equals(object? obj) => obj is Color col && Equals(col);
        public override int GetHashCode() => System.HashCode.Combine(r, g, b);
        public static bool operator ==(Color left, Color right) => left.Equals(right);
        public static bool operator !=(Color left, Color right) => !left.Equals(right);
    }
}