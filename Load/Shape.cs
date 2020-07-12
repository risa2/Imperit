using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class Shape : IConvertibleToWith<State.Shape, bool>
    {
        public IEnumerable<Point>? Border { get; set; }
        public Point? Center { get; set; }
        public State.Shape Convert(int _1, bool _2) => new State.Shape(Border!.Select(pt => pt.Convert()), Center!.Convert());
    }
}