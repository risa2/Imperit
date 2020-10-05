using System.Collections.Generic;
using Imperit.State;

namespace Imperit.Dynamics.Commands
{
	public class Recruit : ICommand
	{
		public readonly int Player;
		public readonly int Land;
		public readonly Army Army;
		public Recruit(int player, int land, Army army)
		{
			Player = player;
			Land = land;
			Army = army;
		}

		public (IAction?, Player) Perform(Player player, IProvinces provinces)
		{
			return player.Id == Player ? (new Actions.Reinforcement(Land, Army), player.ChangeMoney(-Army.Price)) : (null, player);
		}
		public bool Allowed(IReadOnlyList<Player> players, IProvinces provinces)
		{
			return provinces[Land].IsAllyOf(Player) && players[Player].Money >= Army.Price && Army.AnySoldiers;
		}
		public Soldiers Soldiers => Army.Soldiers;
	}
}