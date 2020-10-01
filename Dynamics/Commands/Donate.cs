using System.Collections.Generic;

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
		public bool Allowed(IReadOnlyList<State.Player> players, State.IProvinces provinces) => players[Player].Money >= Amount && Amount > 0;
		public (IAction[], State.Player) Perform(State.Player player, State.IProvinces provinces)
		{
			return (System.Array.Empty<IAction>(), player.Id == Player ? player.Pay(Amount) : player.Id == Recipient ? player.GainMoney(Amount) : player);
		}
	}
}