using Imperit.Load;

namespace Imperit.Services
{
	public interface IServiceIO
	{
		IFile Settings { get; }
		IFile Players { get; }
		IFile Provinces { get; }
		IFile Actions { get; }
		IFile Events { get; }
		IFile Active { get; }
		IFile Password { get; }
		IFile Graph { get; }
		IFile Mountains { get; }
		IFile Shapes { get; }
		IFile Powers { get; }
		IFile Game { get; }
	}
	public class ServiceIO : IServiceIO
	{
		public IFile Settings { get; }
		public IFile Players { get; }
		public IFile Provinces { get; }
		public IFile Actions { get; }
		public IFile Events { get; }
		public IFile Active { get; }
		public IFile Password { get; }
		public IFile Graph { get; }
		public IFile Mountains { get; }
		public IFile Shapes { get; }
		public IFile Powers { get; }
		public IFile Game { get; }
		public ServiceIO(IFile settings, IFile players, IFile provinces, IFile actions, IFile events, IFile active, IFile password, IFile graph, IFile mountains, IFile shapes, IFile powers, IFile game)
		{
			Settings = settings;
			Players = players;
			Provinces = provinces;
			Actions = actions;
			Events = events;
			Active = active;
			Password = password;
			Graph = graph;
			Mountains = mountains;
			Shapes = shapes;
			Powers = powers;
			Game = game;
		}
	}
}