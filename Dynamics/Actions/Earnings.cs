using System.Linq;
using Imperit.State;

namespace Imperit.Dynamics.Actions
{
	public class Earnings : IAction
	{
		public (IAction[], Player) Perform(Player player, Player active, IProvinces provinces)
		{
			return (new[] { this }, player == active ? player.ChangeMoney(provinces.ControlledBy(player.Id).Sum(p => p.Earnings)) : player);
		}
		public byte Priority => 10;
	}
}