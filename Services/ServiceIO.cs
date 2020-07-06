namespace Imperit.Services
{
    public interface IServiceIO
    {
        Load.IFile Settings { get; }
        Load.IFile Players { get; }
        Load.IFile Provinces { get; }
        Load.IFile Actions { get; }
        Load.IFile Events { get; }
        Load.IFile Active { get; }
        Load.IFile Password { get; }
        Load.IFile Graph { get; }
        Load.IFile Mountains { get; }
        Load.IFile Shapes { get; }
        Load.IFile Powers { get; }
    }
    public class ServiceIO : IServiceIO
    {
        public Load.IFile Settings { get; }
        public Load.IFile Players { get; }
        public Load.IFile Provinces { get; }
        public Load.IFile Actions { get; }
        public Load.IFile Events { get; }
        public Load.IFile Active { get; }
        public Load.IFile Password { get; }
        public Load.IFile Graph { get; }
        public Load.IFile Mountains { get; }
        public Load.IFile Shapes { get; }
        public Load.IFile Powers { get; }

        public ServiceIO(Load.IFile settings, Load.IFile players, Load.IFile provinces, Load.IFile actions, Load.IFile events, Load.IFile active, Load.IFile password, Load.IFile graph, Load.IFile mountains, Load.IFile shapes, Load.IFile powers)
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
        }
    }
}