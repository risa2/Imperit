using Imperit.State;

namespace Imperit.Dynamics.Actions
{
	public class Battle : ArmyAction
	{
		public Battle(int province, Army army) : base(province, army) { }
		public override (IAction?, Province) Perform(Province province, Player active)
		{
			return Province == province.Id ? (null, province.AttackedBy(Army)) : (this, province);
		}
		public override (IAction, bool) Interact(ICommand another) => another switch
		{
			Commands.Attack attack when Army.IsAllyOf(attack.Player) && attack.To.Id == Province
				=> (new Battle(Province, Army.Join(attack.Soldiers)), false),
			Commands.Purchase purchase when Army.IsAllyOf(purchase.Player.Id) && purchase.Land == Province
				=> (new Reinforcement(Province, Army), true),
			_ => (this, true)
		};
	}
}