using System.Collections.Generic;
using Imperit.State;

namespace Imperit.Dynamics.Commands
{
	public class Donate : ICommand
	{
		public readonly int Player, Recipient;
		public readonly int Amount;
		public Donate(int player, int recipient, int amount)
		{
			Player = player;
			Recipient = recipient;
			Amount = amount;
		}
		public bool Allowed(IReadOnlyList<Player> players, IProvinces provinces) => players[Player].Money >= Amount && Amount > 0;
		public (IAction[], Player) Perform(Player player, IProvinces provinces)
		{
			return (System.Array.Empty<IAction>(), player.Id == Player ? player.ChangeMoney(-Amount) : player.Id == Recipient ? player.ChangeMoney(Amount) : player);
		}
	}
}