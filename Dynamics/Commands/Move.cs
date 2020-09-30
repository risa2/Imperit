using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
	public abstract class Move : ICommand
	{
		public readonly int Player, From;
		public readonly State.Province To;
		public readonly State.Army Army;
		public Move(int player, int from, State.Province to, State.Army army)
		{
			Player = player;
			From = from;
			To = to;
			Army = army;
		}
		public bool Allowed(IReadOnlyList<State.Player> players, State.IProvinces provinces)
			=> provinces[From].IsControlledBy(Player) && Army.Soldiers.CanMove(provinces, From, To.Id);
		protected abstract Actions.ArmyAction GetMove();
		public (IAction[], State.Province) Perform(State.Province province)
		{
			return province.Id == From ? (new[] { GetMove() }, province.StartMove(To, Army)) : (System.Array.Empty<IAction>(), province);
		}
		public State.Soldiers Soldiers => Army.Soldiers;
	}
}