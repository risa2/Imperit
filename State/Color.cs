using System.Globalization;

namespace Imperit.State
{
    public readonly struct Color : System.IEquatable<Color>
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
        public bool Equals(Color other) => r == other.r && g == other.g && b == other.b;
        public override bool Equals(object? obj) => obj is Color col && Equals(col);
        public override int GetHashCode() => System.HashCode.Combine(r, g, b);
        public static bool operator ==(Color left, Color right) => left.Equals(right);
        public static bool operator !=(Color left, Color right) => !left.Equals(right);
    }
}