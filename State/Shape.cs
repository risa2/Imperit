using System.Collections;
using System.Collections.Generic;

namespace Imperit.State
{
    public class Shape : IEnumerable<Point>
    {
        readonly IEnumerable<Point> border;
        public readonly Point Center;
        public Shape(IEnumerable<Point> points, Point center)
        {
            border = points;
            Center = center;
        }
        public IEnumerator<Point> GetEnumerator() => border.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => border.GetEnumerator();
    }
}