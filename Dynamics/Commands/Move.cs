using System.Collections.Generic;
using Imperit.State;

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
		protected abstract Actions.ArmyAction Action { get; }
		public (IAction?, Province) Perform(Province province)
		{
			return province.Id == From ? (Action, province.StartMove(To, Army.Soldiers)) : (null, province);
		}
		public Soldiers Soldiers => Army.Soldiers;
	}
}