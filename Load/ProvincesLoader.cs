using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class ProvincesLoader
    {
        readonly Writer<Province, State.Province, (State.Settings, IReadOnlyList<State.Player>, IReadOnlyList<State.Shape>)> loader;
        readonly State.Graph graph;
        public ProvincesLoader(IFile provinces, IFile graphfile, IFile shapes, State.Settings settings, IReadOnlyList<State.Player> players)
        {
            var shapelist = new Loader<Shape, State.Shape, bool>(shapes, false).Load().ToArray();
            loader = new Writer<Province, State.Province, (State.Settings, IReadOnlyList<State.Player>, IReadOnlyList<State.Shape>)>(provinces, (settings, players, shapelist), Province.FromProvince);
            graph = new Loader<Graph, State.Graph, bool>(graphfile, false).LoadOne();
        }
        public State.Provinces Load() => new State.Provinces(loader.Load().ToArray(), graph);
        public void Save(State.Provinces saved) => loader.Save(saved);
    }
}