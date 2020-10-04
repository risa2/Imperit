using Imperit.State;
using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
	public abstract class Move : ICommand
	{
		public readonly int Player, From;
		public readonly Province To;
		public readonly Army Army;
		public Move(int player, int from, Province to, Army army)
		{
			Player = player;
			From = from;
			To = to;
			Army = army;
		}
		public bool Allowed(IReadOnlyList<Player> players, IProvinces provinces)
			=> provinces[From].IsAllyOf(Player) && Army.CanMove(provinces, From, To.Id);
		protected abstract Actions.ArmyAction GetMove();
		public (IAction[], Province) Perform(Province province)
		{
			return province.Id == From ? (new[] { GetMove() }, province.StartMove(To, Army.Soldiers)) : (System.Array.Empty<IAction>(), province);
		}
		public Soldiers Soldiers => Army.Soldiers;
	}
}