using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Donation : ICommand
    {
        public State.Player Player { get; }
        public readonly State.Player Recipient;
        public readonly uint Amount;
        public Donation(State.Player player, State.Player recipient, uint amount)
        {
            Player = player;
            Recipient = recipient;
            Amount = amount;
        }
        public bool Allowed => Player.Money >= Amount && Amount > 0;
        public IAction Consequences => new Actions.Nothing();
        public void Do(IList<State.Player> players, State.Provinces provinces)
        {
            players[Player.Id] = Player.Pay(Amount);
            players[Recipient.Id] = Recipient.GainMoney(Amount);
        }
    }
}