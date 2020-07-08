using System.Collections.Generic;

namespace Imperit.Services
{
    public interface IMap
    {
        IEnumerable<State.MountainRange> Mountains { get; }
        IEnumerable<State.Shape> Shapes { get; }
    }
    public class Map : IMap
    {
        readonly Load.IFile input;
        readonly IProvincesLoader pl;
        public Map(IServiceIO io, IProvincesLoader pl)
        {
            input = io.Shapes;
            this.pl = pl;
            Mountains = new Load.Loader<Load.MountainRange, State.MountainRange, bool>(io.Mountains, false).Load();
        }
        public IEnumerable<State.MountainRange> Mountains { get; }
        public IEnumerable<State.Shape> Shapes => new Load.Loader<Load.Shape, State.Shape, IReadOnlyList<State.Province>>(input, pl.Provinces).Load();
    }
}