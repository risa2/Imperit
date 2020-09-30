using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
	public class Recruit : ICommand
	{
		public readonly int Player;
		public readonly int Land;
		public readonly State.Army Army;
		public Recruit(int player, int land, State.Army army)
		{
			Player = player;
			Land = land;
			Army = army;
		}

		public (IAction[], State.Player) Perform(State.Player player, State.IProvinces provinces)
		{
			return player.Id == Player
				? (new[] { new Actions.Reinforcement(Land, Army) }, player.Pay(Army.Soldiers.Price))
				: (System.Array.Empty<IAction>(), player);
		}
		public bool Allowed(IReadOnlyList<State.Player> players, State.IProvinces provinces)
		{
			return provinces[Land].IsControlledBy(Player) && players[Player].Money >= Army.Soldiers.Price && Army.Soldiers.Any;
		}
	}
}