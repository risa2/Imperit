using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class MountainRange : IConvertibleToWith<State.MountainRange, State.Settings>
    {
        public IEnumerable<Point>? Line { get; set; }
        public State.MountainRange Convert(int i, State.Settings settings)
        {
            return new State.MountainRange(Line!.Select(pt => pt.Convert()).ToArray(), settings);
        }
    }
}