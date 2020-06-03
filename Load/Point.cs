namespace Imperit.Load
{
    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
        public State.Point Convert() => new State.Point(x, y);
    }
}