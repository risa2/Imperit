namespace Imperit.Dynamics.Commands
{
	public class Reinforce : Move
	{
		public Reinforce(int player, int from, State.Province to, State.Army army) : base(player, from, to, army) { }
		protected override Actions.ArmyAction GetMove() => new Actions.Reinforcement(To.Id, Army);
	}
}