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
        Load.IFile input;
        IEnumerable<State.MountainRange> mountains;
        IProvincesLoader pl;
        public Map(IServiceIO io, IProvincesLoader pl)
        {
            input = io.Shapes;
            this.pl = pl;
            mountains = new Load.Loader<Load.MountainRange, State.MountainRange, bool>(io.Mountains, false).Load();
        }
        public IEnumerable<State.Shape> Shapes => new Load.Loader<Load.Shape, State.Shape, State.Provinces>(input, pl.Provinces).Load();
        public IEnumerable<State.MountainRange> Mountains => mountains;
    }
}