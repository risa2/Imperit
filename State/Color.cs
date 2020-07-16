namespace Imperit.State
{
    public readonly struct Color : System.IEquatable<Color>
    {
        static byte FromHex(string s) => byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
        public readonly byte r, g, b;
        public Color(byte R, byte G, byte B)
        {
            r = R;
            g = G;
            b = B;
        }
        public override string ToString() => "#" + r.ToHexString() + g.ToHexString() + b.ToHexString();
        public static Color Parse(string str) => new Color(FromHex(str[1..3]), FromHex(str[3..5]), FromHex(str[5..7]));
        public Color Darken(byte light) => new Color((byte)(r * light / 255), (byte)(g * light / 255), (byte)(b * light / 255));

        public bool Equals(Color other) => r == other.r && g == other.g && b == other.b;
        public override bool Equals(object? obj) => obj is Color col && Equals(col);
        public override int GetHashCode() => System.HashCode.Combine(r, g, b);
        public static bool operator ==(Color left, Color right) => left.Equals(right);
        public static bool operator !=(Color left, Color right) => !left.Equals(right);
    }
}