using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
    public class Graph
    {
        readonly int[] edges, starts;
        public Graph(int[] edges, int[] starts)
        {
            this.edges = edges;
            this.starts = starts;
        }
        public bool Passable(int from, int to) => NeighborsOf(from).Contains(to);
        public int NeighborCount(int vertex) => starts[vertex + 1] - starts[vertex];
        public IEnumerable<int> NeighborsOf(int vertex) => edges.Take(starts[vertex + 1]).Skip(starts[vertex]);
        public int Vertices => starts.Length - 1;
    }
}