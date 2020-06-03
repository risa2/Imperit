using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
    public class PortRenewal : IAction
    {
        public readonly int Port;
        public PortRenewal(int port) => Port = port;
        public IAction Do(IList<State.Player> players, State.Provinces provinces, int active)
        {
            provinces[Port] = (provinces[Port] as State.Port)!.Renew();
            return new PortRenewal(Port);
        }
    }
}