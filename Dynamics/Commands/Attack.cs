namespace Imperit.Dynamics.Commands
{
	public class Attack : Move
	{
		public Attack(int player, int from, State.Province to, State.Army army) : base(player, from, to, army) { }
		protected override Actions.ArmyAction GetMove() => new Actions.Battle(To.Id, Army);
	}
}