using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
    public class Graph : IReadOnlyList<IEnumerable<int>>
    {
        readonly int[] edges, starts;
        public Graph(int[] edges, int[] starts)
        {
            this.edges = edges;
            this.starts = starts;
        }
        public bool Passable(int from, int to) => this[from].Contains(to);
        public uint NeighborCount(int vertex) => (uint)(starts[vertex + 1] - starts[vertex]);
        public IEnumerable<int> this[int vertex] => edges.Take(starts[vertex + 1]).Skip(starts[vertex]);
        public int Count => starts.Length - 1;
        public IEnumerator<IEnumerable<int>> GetEnumerator() => Enumerable.Range(0, Count).Select(i => this[i]).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}