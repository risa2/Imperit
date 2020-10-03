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
		int? Distance(int from, int to, int limit)
		{
			var stack = new List<(int Pos, int Distance)>() { (from, 0) };
			var visited = new bool[Count];
			visited[from] = true;
			for (int i = 0; i < stack.Count; ++i)
			{
				if (stack[i].Pos == to)
				{
					return stack[i].Distance;
				}
				if (stack[i].Distance < limit)
				{
					foreach (int vertex in this[stack[i].Pos].Where(n => !visited[n]))
					{
						stack.Add((vertex, stack[i].Distance + 1));
						visited[vertex] = true;
					}
				}
			}
			return null;
		}
		public int? Distance(int from, int to) => Distance(from, to, int.MaxValue);
		public bool Passable(int from, int to) => Distance(from, to, 1) is int;
		public int NeighborCount(int vertex) => starts[vertex + 1] - starts[vertex];
		public IEnumerable<int> this[int vertex] => edges.Take(starts[vertex + 1]).Skip(starts[vertex]);
		public int Count => starts.Length - 1;
		public IEnumerator<IEnumerable<int>> GetEnumerator() => Enumerable.Range(0, Count).Select(i => this[i]).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}