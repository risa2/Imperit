using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class Graph : IConvertibleToWith<State.Graph, bool>
    {
        public IReadOnlyList<int[]>? Neighbors { get; set; }
        public State.Graph Convert(int useless, bool even_more_useless)
        {
            var edges = Neighbors!.SelectMany(n => n).ToArray();
            var starts = new int[Neighbors!.Count + 1];
            starts[0] = 0;
            for (int i = 1; i < starts.Length; ++i)
            {
                starts[i] = starts[i - 1] + Neighbors![i - 1].Length;
            }
            return new State.Graph(edges, starts);
        }
    }
}