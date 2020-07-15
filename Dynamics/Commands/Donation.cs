using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Donation : ICommand
    {
        public readonly int Player, Recipient;
        public readonly uint Amount;
        public Donation(int player, int recipient, uint amount)
        {
            Player = player;
            Recipient = recipient;
            Amount = amount;
        }
        public bool Allowed(IReadOnlyList<State.Player> players, State.Provinces provinces) => players[Player].Money >= Amount && Amount > 0;
        public (IAction[], State.Player) Do(State.Player player, State.Provinces provinces)
        {
            return (System.Array.Empty<IAction>(), player.Id == Player ? player.Pay(Amount) : player.Id == Recipient ? player.GainMoney(Amount) : player);
        }
    }
}