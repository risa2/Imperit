using System;
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
		public bool Passable(int from, int to, int limit, Func<int, int, int> difficulty)
		{
			var stack = new List<(int Pos, int Distance)>() { (from, 0) };
			var visited = new bool[Count];
			visited[from] = true;
			for (int i = 0; i < stack.Count; ++i)
			{
				if (stack[i].Pos == to)
				{
					return true;
				}
				foreach (int vertex in this[stack[i].Pos].Where(n => !visited[n]))
				{
					if (stack[i].Distance + difficulty(stack[i].Pos, vertex) <= limit)
					{
						stack.Add((vertex, stack[i].Distance + difficulty(stack[i].Pos, vertex)));
						visited[vertex] = true;
					}
				}
			}
			return false;
		}
		public int NeighborCount(int vertex) => starts[vertex + 1] - starts[vertex];
		public IEnumerable<int> this[int vertex] => edges.Take(starts[vertex + 1]).Skip(starts[vertex]);
		public int Count => starts.Length - 1;
		public IEnumerator<IEnumerable<int>> GetEnumerator() => Enumerable.Range(0, Count).Select(i => this[i]).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}