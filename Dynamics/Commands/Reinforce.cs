using Imperit.Dynamics.Actions;
using Imperit.State;

namespace Imperit.Dynamics.Commands
{
	public class Reinforce : Move
	{
		public Reinforce(int player, int from, Province to, Army army) : base(player, from, to, army) { }
		protected override Movement Action => new Reinforcement(To.Id, Army);
	}
}