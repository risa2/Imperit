namespace Imperit.Load
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public State.Point Convert() => new State.Point(X, Y);
    }
}