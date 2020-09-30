using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
	public interface IProvinces : IReadOnlyList<Province>
	{
		new Province this[int i] { get; set; }
		Province IReadOnlyList<Province>.this[int index] => this[index];
		bool Passable(int from, int to);
		uint? Distance(int from, int to);
		uint NeighborCount(int prov);
		IEnumerable<Province> NeighborsOf(int prov);
		Provinces With(Province[] provinces);
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
		public bool Passable(int from, int to) => graph.Passable(from, to);
		public uint? Distance(int from, int to) => graph.Distance(from, to);
		public uint NeighborCount(int id) => graph.NeighborCount(id);
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