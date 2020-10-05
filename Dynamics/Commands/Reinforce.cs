using Imperit.State;

namespace Imperit.Dynamics.Commands
{
	public class Reinforce : Move
	{
		public Reinforce(int player, int from, Province to, Army army) : base(player, from, to, army) { }
		protected override Actions.ArmyAction GetMove() => new Actions.Reinforcement(To.Id, Army);
	}
}