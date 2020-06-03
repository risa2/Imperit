using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
    public class Shape : IEnumerable<Point>
    {
        readonly IEnumerable<Point> border;
        public Point Center { get; }
        public Province Province { get; }
        public Shape(IEnumerable<Point> points, Point center, Province province)
        {
            border = points;
            Center = center;
            Province = province;
        }
        public IEnumerator<Point> GetEnumerator() => border.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => (border as IEnumerable).GetEnumerator();
    }
}