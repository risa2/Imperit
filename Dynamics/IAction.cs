using Imperit.State;
using System.Collections.Generic;

namespace Imperit.Dynamics
{
	public interface IAction
	{
		(IAction?, Province) Perform(Province province, Player active) => (this, province);
		(IAction?, Player) Perform(Player player, Player active, IProvinces provinces) => (this, player);
		(IAction?, bool) Interact(ICommand another) => (this, true);
		bool Allows(ICommand another, IReadOnlyList<Player> players, IProvinces provinces) => true;
		byte Priority { get; }
	}
}