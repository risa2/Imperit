using System.Linq;

namespace Imperit.Dynamics.Actions
{
	public class Earnings : IAction
	{
		public (IAction[], State.Player) Perform(State.Player player, State.Player active, State.IProvinces provinces)
		{
			return (new[] { this }, player == active ? player.ChangeMoney(provinces.ControlledBy(player.Id).Sum(p => p.Earnings)) : player);
		}
		public byte Priority => 10;
	}
}