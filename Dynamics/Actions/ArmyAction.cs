using Imperit.State;

namespace Imperit.Dynamics.Actions
{
	public abstract class ArmyAction : IAction
	{
		public readonly int Province;
		public readonly Army Army;
		public ArmyAction(int province, Army army)
		{
			Province = province;
			Army = army;
		}
		public abstract (IAction[], Province) Perform(Province province, Player active);
		public abstract (IAction, bool) Interact(ICommand another);
		public Soldiers Soldiers => Army.Soldiers;
		public byte Priority => 50;
	}
}