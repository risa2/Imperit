namespace Imperit.Dynamics.Commands
{
    public class Reinforcement : Move
    {
        public Reinforcement(State.Provinces prov, State.Player player, State.Province from, State.Province to, State.IArmy army) : base(prov, player, from, to, army) { }
        public override IAction Consequences => new Actions.Reinforcement(To.Id, Army);
    }
}