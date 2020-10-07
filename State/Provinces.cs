using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
	public interface IProvinces : IReadOnlyList<Province>
	{
		new Province this[int i] { get; set; }
		Province IReadOnlyList<Province>.this[int index] => this[index];
		bool Passable(int from, int to, int distance, Func<Province, Province, int> difficulty);
		bool Passable(int from, int to, int distance = 1) => Passable(from, to, distance, (a, b) => 1);
		int NeighborCount(int prov);
		IEnumerable<Province> NeighborsOf(int prov);
		Provinces With(Province[] provinces);
		IEnumerable<Province> ControlledBy(int id) => this.Where(p => p.IsAllyOf(id));
	}
	public class Provinces : IProvinces
	{
		readonly Province[] provinces;
		readonly Graph graph;
		public Provinces(Province[] provinces, Graph graph)
		{
			this.provinces = provinces;
			this.graph = graph;
		}
		public Provinces With(Province[] new_provinces) => new Provinces(new_provinces, graph);
		public bool Passable(int from, int to, int distance, Func<Province, Province, int> difficulty)
		{
			return graph.Passable(from, to, distance, (start, dest) => difficulty(this[start], this[dest]));
		}
		public int NeighborCount(int id) => graph.NeighborCount(id);
		public IEnumerable<Province> NeighborsOf(int id) => graph[id].Select(vertex => provinces[vertex]);

		public Province this[int key]
		{
			get => provinces[key];
			set => provinces[key] = value;
		}
		public IEnumerator<Province> GetEnumerator() => (provinces as IEnumerable<Province>).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => provinces.GetEnumerator();
		public int Count => provinces.Length;
	}
}