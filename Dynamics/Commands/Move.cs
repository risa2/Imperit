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
		public virtual bool Allowed(IReadOnlyList<Player> players, IProvinces provinces)
		{
			return provinces[From].IsAllyOf(Player) && Army.CanMove(provinces, From, To.Id) && provinces[To.Id].Subtract(Army.Soldiers).CanSoldiersSurvive;
		}
		protected abstract Actions.Movement Action { get; }
		public (IAction?, Province) Perform(Province province)
		{
			return province.Id == From ? (Action, province.Subtract(Army.Soldiers)) : (null, province);
		}
		public Soldiers Soldiers => Army.Soldiers;
	}
}