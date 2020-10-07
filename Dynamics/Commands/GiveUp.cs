using System.Collections.Generic;
using Imperit.State;

namespace Imperit.Dynamics.Commands
{
	public class GiveUp : ICommand
	{
		public readonly int Player;
		public GiveUp(int player) => Player = player;
		public bool Allowed(IReadOnlyList<Player> players, IProvinces provinces) => true;
		public (IAction?, Province) Perform(Province province) => (null, province.IsAllyOf(Player) ? province.Revolt() : province);
		public (IAction?, Player) Perform(Player player, IProvinces provinces) => (null, player.Id == Player ? player.Die() : player);

	}
}
