namespace Imperit.State
{
    public readonly struct Color
    {
        private static byte FromHex(string s) => byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
        public readonly byte r, g, b;
        public Color(byte R, byte G, byte B)
        {
            r = R;
            g = G;
            b = B;
        }
        override public string ToString() => "#" + r.ToString("x2") + g.ToString("x2") + b.ToString("x2");
        public static Color Parse(string str) => new Color(FromHex(str[1..3]), FromHex(str[3..5]), FromHex(str[5..7]));
        private static byte Avg(byte x, byte y) => (byte)((x + y) / 2);
        public Color Darken(byte light) => new Color((byte)(r * light / 255), (byte)(g * light / 255), (byte)(b * light / 255));
    }
}