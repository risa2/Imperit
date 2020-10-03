using System.Collections.Generic;
using Imperit.State;

namespace Imperit.Dynamics
{
	public interface IAction
	{
		(IAction[], Province) Perform(Province province, Player active) => (new[] { this }, province);
		(IAction[], Player) Perform(Player player, Player active, IProvinces provinces) => (new[] { this }, player);
		(IAction?, bool) Interact(ICommand another) => (this, true);
		bool Allows(ICommand another, IReadOnlyList<Player> players, IProvinces provinces) => true;
		byte Priority { get; }
	}
}