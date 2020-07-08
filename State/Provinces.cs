using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
    public class Provinces : IArray<Province>
    {
        readonly Province[] provinces;
        public readonly Graph Graph;
        public Provinces(Province[] prov, Graph graph)
        {
            provinces = prov;
            Graph = graph;
        }
        public uint CanMove(Province from, Province to) => Graph.Passable(from.Id, to.Id) ? from.CanMoveTo(to) : 0;
        public uint CanMove(int from, int to) => Graph.Passable(from, to) ? provinces[from].CanMoveTo(provinces[to]) : 0;
        public uint NeighborCount(Province prov) => Graph.NeighborCount(prov.Id);
        public IEnumerable<Province> NeighborsOf(int prov) => Graph[prov].Select(vertex => provinces[vertex]);
        public IEnumerable<Province> NeighborsOf(Province prov) => NeighborsOf(prov.Id);
        public IEnumerable<Province> ControlledBy(Player player) => provinces.Where(prov => prov.IsControlledBy(player));

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