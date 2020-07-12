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
        public Map(IServiceIO io)
        {
            Mountains = new Load.Loader<Load.MountainRange, State.MountainRange, bool>(io.Mountains, false).Load();
            Shapes = new Load.Loader<Load.Shape, State.Shape, bool>(io.Shapes, false).Load();
        }
        public IEnumerable<State.MountainRange> Mountains { get; }
        public IEnumerable<State.Shape> Shapes { get; }
    }
}