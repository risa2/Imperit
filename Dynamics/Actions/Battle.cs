using System;

namespace Imperit.Dynamics.Actions
{
	public class Battle : ArmyAction
	{
		public Battle(int province, State.Army army) : base(province, army) { }
		public override (IAction[], State.Province) Perform(State.Province province, State.Player active)
		{
			return Province == province.Id ? (Array.Empty<IAction>(), province.AttackedBy(Army)) : (new[] { this }, province);
		}
		public override (IAction, bool) Interact(ICommand another) => another switch
		{
			Commands.Attack attack when Army.IsControlledBy(attack.Player) && attack.To.Id == Province
				=> (new Battle(Province, Army.Join(attack.Army)), false),
			Commands.Purchase purchase when Army.IsControlledBy(purchase.Player.Id) && purchase.Land == Province
				=> (new Reinforcement(Province, Army), true),
			_ => (this, true)
		};
	}
}