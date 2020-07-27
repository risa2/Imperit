using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
    public interface IProvinces : IReadOnlyList<Province>
    {
        new Province this[int i] { get; set; }
        Province IReadOnlyList<Province>.this[int index] => this[index];
        uint CanMove(int from, int to);
        uint NeighborCount(int prov);
        IEnumerable<Province> NeighborsOf(int prov);
        Provinces With(Province[] provinces);
    }
    public class Provinces : IProvinces
    {
        readonly Province[] provinces;
        public readonly Graph Graph;
        public Provinces(Province[] prov, Graph graph)
        {
            provinces = prov;
            Graph = graph;
        }
        public Provinces With(Province[] new_provinces) => new Provinces(new_provinces, Graph);
        public uint CanMove(int from, int to) => Graph.Passable(from, to) ? provinces[from].CanMoveTo(provinces[to]) : 0;
        public uint NeighborCount(int id) => Graph.NeighborCount(id);
        public IEnumerable<Province> NeighborsOf(int id) => Graph[id].Select(vertex => provinces[vertex]);

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