using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class Shape : IConvertibleToWith<State.Shape, State.Provinces>
    {
        public IEnumerable<Point>? Border { get; set; }
        public Point? Center { get; set; }
        public State.Shape Convert(int i, State.Provinces prv) => new State.Shape(Border!.Select(pt => pt.Convert()), Center!.Convert(), prv[i]);
    }
}