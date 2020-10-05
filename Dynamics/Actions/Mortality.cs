using System.Linq;
using Imperit.State;

namespace Imperit.Dynamics.Actions
{
	public class Mortality : IAction
	{
		public (IAction?, Player) Perform(Player player, Player active, IProvinces provinces)
		{
			return (this, provinces.Any(prov => prov.IsAllyOf(player.Id)) ? player : player.Die());
		}
		public (IAction?, Province) Perform(Province province, Player active)
		{
			if (province is Sea Sea && Sea.Occupied && !Sea.Soldiers.Any)
			{
				return (this, Sea.Revolt());
			}
			return (this, province.CanSoldiersSurvive ? province : province.Revolt());
		}
		public byte Priority => 200;
	}
}