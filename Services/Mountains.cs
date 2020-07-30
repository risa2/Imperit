using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IMountains : IReadOnlyList<State.MountainRange> { }
    public class Mountains : IMountains
    {
        readonly State.MountainRange[] mountains;
        public Mountains(IServiceIO io, ISettingsLoader sl)
        {
            mountains = new Load.Loader<Load.MountainRange, State.MountainRange, State.Settings>(io.Mountains, sl.Settings).Load().ToArray();
        }
        public State.MountainRange this[int i] => mountains[i];
        public int Count => mountains.Length;
        public IEnumerator<State.MountainRange> GetEnumerator()
        {
            return (mountains as IEnumerable<State.MountainRange>).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}