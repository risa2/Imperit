using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class Shape : IConvertibleToWith<State.Shape, IReadOnlyList<State.Province>>
    {
        public IEnumerable<Point>? Border { get; set; }
        public Point? Center { get; set; }
        public State.Shape Convert(int i, IReadOnlyList<State.Province> prv) => new State.Shape(Border!.Select(pt => pt.Convert()), Center!.Convert(), prv[i]);
    }
}