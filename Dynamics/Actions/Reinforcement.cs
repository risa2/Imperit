namespace Imperit.Dynamics.Actions
{
	public class Reinforcement : ArmyAction
	{
		public Reinforcement(int province, State.Army army) : base(province, army) { }
		public override (IAction[], State.Province) Perform(State.Province province, State.Player active)
		{
			return Province == province.Id ? (System.Array.Empty<IAction>(), province.ReinforcedBy(Army)) : (new[] { this }, province);
		}
		public override (IAction, bool) Interact(ICommand another) => another switch
		{
			Commands.Reinforce reinf when Army.IsControlledBy(reinf.Player) && reinf.To.Id == Province
				=> (new Reinforcement(Province, Army.Join(reinf.Army)), false),
			Commands.Recruit recr when Army.IsControlledBy(recr.Player) && recr.Land == Province
				=> (new Reinforcement(Province, Army.Join(recr.Army)), false),
			_ => (this, true)
		};
	}
}