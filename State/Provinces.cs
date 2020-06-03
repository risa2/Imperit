using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
    public class Provinces : IEnumerable<State.Province>, IReadOnlyList<State.Province>
    {
        private readonly Province[] provinces;
        public readonly Graph Graph;
        public Provinces(Province[] provinces, Graph graph)
        {
            this.provinces = provinces;
            this.Graph = graph;
        }
        public uint CanMove(Province from, Province to) => Passable(from, to) ? from.CanMoveTo(to) : 0;
        public bool Passable(Province from, Province to) => Graph.Passable(from.Id, to.Id);
        public int NeighborCount(Province prov) => Graph.NeighborCount(prov.Id);
        public IEnumerable<Province> NeighborsOf(Province prov) => Graph.NeighborsOf(prov.Id).Select(vertex => provinces[vertex]);
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