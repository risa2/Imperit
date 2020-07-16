namespace Imperit.State
{
    public readonly struct Point : System.IEquatable<Point>
    {
        public readonly double x, y;
        public Point(double X, double Y) => (x, y) = (X, Y);
        public bool Equals(Point other) => x == other.x && y == other.y;
        public override bool Equals(object? obj) => obj is Color col && Equals(col);
        public override int GetHashCode() => System.HashCode.Combine(x, y);
        public static bool operator ==(Point left, Point right) => left.Equals(right);
        public static bool operator !=(Point left, Point right) => !left.Equals(right);
    }
}