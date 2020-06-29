using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class ProvincesLoader
    {
        readonly Writer<Province, State.Province, (State.Settings, IReadOnlyList<State.Player>)> provinces_loader;
        readonly Loader<Graph, State.Graph, bool> graph_loader;
        public ProvincesLoader(IFile provinces, IFile graph, State.Settings settings, IReadOnlyList<State.Player> players)
        {
            provinces_loader = new Writer<Province, State.Province, (State.Settings, IReadOnlyList<State.Player>)>(provinces, (settings, players), Province.FromProvince);
            graph_loader = new Loader<Graph, State.Graph, bool>(graph, false);
        }
        public State.Provinces Load() => new State.Provinces(provinces_loader.Load().ToArray(), graph_loader.LoadOne());
        public void Save(State.Provinces saved) => provinces_loader.Save(saved);
    }
}