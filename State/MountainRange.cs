using System.Collections.Generic;

namespace Imperit.State
{
    public class MountainRange : IEnumerableImpl<Point>
    {
        readonly IEnumerable<Point> line;
        public MountainRange(IEnumerable<Point> line) => this.line = line;
        public IEnumerator<Point> GetEnumerator() => line.GetEnumerator();
    }
}