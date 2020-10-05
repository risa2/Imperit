using System;
using Imperit.Dynamics.Commands;
using Imperit.State;

namespace Imperit.Dynamics.Actions
{
	public class Reinforcement : ArmyAction
	{
		public Reinforcement(int province, Army army) : base(province, army) { }
		public override (IAction[], Province) Perform(Province province, Player active)
		{
			return Province == province.Id ? (Array.Empty<IAction>(), province.ReinforcedBy(Soldiers)) : (new[] { this }, province);
		}
		public override (IAction, bool) Interact(ICommand another) => another switch
		{
			Reinforce reinf when Army.IsAllyOf(reinf.Player) && reinf.To.Id == Province
				=> (new Reinforcement(Province, Army.Join(reinf.Soldiers)), false),
			Recruit recr when Army.IsAllyOf(recr.Player) && recr.Land == Province
				=> (new Reinforcement(Province, Army.Join(recr.Soldiers)), false),
			_ => (this, true)
		};
	}
}