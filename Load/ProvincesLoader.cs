using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
	public class ProvincesLoader
	{
		readonly JsonWriter<JsonProvince, State.Province, (State.Settings, IReadOnlyList<State.Player>, IReadOnlyList<State.Shape>)> loader;
		readonly State.Graph graph;
		public ProvincesLoader(IFile provinces, IFile graphfile, IFile shapes, State.Settings settings, IReadOnlyList<State.Player> players)
		{
			var shapelist = new JsonLoader<JsonShape, State.Shape, bool>(shapes, false).Load().ToArray();
			loader = new JsonWriter<JsonProvince, State.Province, (State.Settings, IReadOnlyList<State.Player>, IReadOnlyList<State.Shape>)>(provinces, (settings, players, shapelist), JsonProvince.From);
			graph = new JsonLoader<JsonGraph, State.Graph, bool>(graphfile, false).LoadOne();
		}
		public State.Provinces Load() => new State.Provinces(loader.Load().ToArray(), graph);
		public void Save(State.Provinces saved) => loader.Save(saved);
	}
}