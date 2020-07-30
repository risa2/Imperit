using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class Shape : IConvertibleToWith<State.Shape, bool>
    {
        public IEnumerable<Point>? Border { get; set; }
        public Point? Center { get; set; }
        public State.Shape Convert(int _, bool __)
        {
            return new State.Shape(Border!.Select(pt => pt.Convert()).ToArray(), Center!.Convert());
        }
    }
}