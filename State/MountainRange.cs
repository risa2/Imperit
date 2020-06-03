using System.Collections;
using System.Collections.Generic;

namespace Imperit.State
{
    public class MountainRange : IEnumerable<Point>
    {
        readonly IEnumerable<Point> line;
        public MountainRange(IEnumerable<Point> line) => this.line = line;
        public IEnumerator<Point> GetEnumerator() => (line as IEnumerable<Point>).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => line.GetEnumerator();
    }
}