using System.Collections.Generic;

namespace Imperit.Dynamics.Actions
{
	public class Reinforcement : ArmyAction
	{
		public Reinforcement(int province, State.Army army) : base(province, army) { }
		public override (IAction[], State.Province) Perform(State.Province province, State.Player active)
		{
			return Province == province.Id ? (System.Array.Empty<IAction>(), province.ReinforcedBy(Army)) : (new[] { this }, province);
		}
		public override (IAction, bool) Interact(ICommand another, IReadOnlyList<State.Player> players, State.IProvinces provinces)
		{
			if (another is Commands.Reinforce reinf && Army.IsControlledBy(reinf.Player) && reinf.To.Id == Province)
			{
				return (new Reinforcement(Province, Army.Join(reinf.Army)), false);
			}
			if (another is Commands.Recruit recr && Army.IsControlledBy(recr.Player) && recr.Land == Province)
			{
				return (new Reinforcement(Province, Army.Join(recr.Army)), false);
			}
			return (this, true);
		}
	}
}