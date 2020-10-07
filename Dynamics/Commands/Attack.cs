using System.Collections.Generic;
using Imperit.State;
using Imperit.Dynamics.Actions;

namespace Imperit.Dynamics.Commands
{
	public class Attack : Move
	{
		public Attack(int player, int from, Province to, Army army) : base(player, from, to, army) { }
		protected override Movement Action => new Fight(To.Id, Army);
		public override bool Allowed(IReadOnlyList<Player> players, IProvinces provinces) => Army.AttackPower > 0 && base.Allowed(players, provinces);
	}
}