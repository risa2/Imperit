namespace Imperit.Dynamics.Commands
{
    public class Reinforcement : Move
    {
        public Reinforcement(int player, int from, State.Province to, State.IArmy army) : base(player, from, to, army) { }
        protected override Actions.Move GetMove() => new Actions.Reinforcement(To.Id, Army);
    }
}