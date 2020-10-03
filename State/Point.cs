namespace Imperit.State
{
	public readonly struct Point : System.IEquatable<Point>, System.IComparable<Point>
	{
		public readonly double x, y;
		public Point(double X, double Y) => (x, y) = (X, Y);
		public int CompareTo(Point p2) => ((x * x) + (y * y)).CompareTo((p2.x * p2.x) + (p2.y * p2.y));
		public bool Equals(Point p2) => (x, y) == (p2.x, p2.y);
		public override bool Equals(object? obj) => obj is Color col && Equals(col);
		public override int GetHashCode() => System.HashCode.Combine(x, y);
		public static bool operator ==(Point left, Point right) => left.Equals(right);
		public static bool operator !=(Point left, Point right) => !left.Equals(right);
	}
}