using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
	public abstract class ArmyAction : IAction
	{
		public readonly int Province;
		public readonly State.Army Army;
		public ArmyAction(int province, State.Army army)
		{
			Province = province;
			Army = army;
		}
		public abstract (IAction[], State.Province) Perform(State.Province province, State.Player active);
		public abstract (IAction, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.IProvinces provinces);
		public State.Soldiers Soldiers => Army.Soldiers;
		public byte Priority => 50;
	}
}