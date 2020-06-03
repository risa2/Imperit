using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class MountainRange : IConvertibleToWith<State.MountainRange, bool>
    {
        public IEnumerable<Point>? Line { get; set; }
        public State.MountainRange Convert(int i, bool useless) => new State.MountainRange(Line!.Select(pt => pt.Convert()));
    }
}