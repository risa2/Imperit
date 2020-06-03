namespace Imperit.State
{
    public readonly struct Point
    {
        public readonly double x, y;
        public Point(double X, double Y) => (x, y) = (X, Y);
    }
}