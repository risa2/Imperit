using Imperit.State;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
	public class ProvincesJsonLoader
	{
		readonly JsonWriter<JsonProvince, Province, (Settings, IReadOnlyList<Player>, IReadOnlyList<Shape>)> loader;
		readonly Graph graph;
		public ProvincesJsonLoader(IFile provinces, IFile graphfile, IFile shapes, Settings settings, IReadOnlyList<Player> players)
		{
			var shapelist = new JsonLoader<JsonShape, Shape, bool>(shapes, false).Load().ToArray();
			loader = new JsonWriter<JsonProvince, Province, (Settings, IReadOnlyList<Player>, IReadOnlyList<Shape>)>(provinces, (settings, players, shapelist), JsonProvince.From);
			graph = new JsonLoader<JsonGraph, Graph, bool>(graphfile, false).LoadOne();
		}
		public Provinces Load() => new Provinces(loader.Load().ToArray(), graph);
		public void Save(Provinces saved) => loader.Save(saved);
	}
}