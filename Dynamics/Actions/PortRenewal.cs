namespace Imperit.Dynamics.Actions
{
    public class PortRenewal : IAction
    {
        public (IAction[], State.Province) Do(State.Province province, State.Player active)
        {
            return (new[] { this }, province is State.Port Port ? Port.Renew() : province);
        }
        public byte Priority => 0;
    }
}