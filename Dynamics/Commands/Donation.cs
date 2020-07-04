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
        public bool Allowed(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces) => players[Player].Money >= Amount && Amount > 0;
        public IAction Do(State.Settings settings, IArray<State.Player> players, State.Provinces provinces)
        {
            players[Player] = players[Player].Pay(Amount);
            players[Recipient] = players[Recipient].GainMoney(Amount);
            return new Actions.Nothing();
        }
    }
}