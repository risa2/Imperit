using System.Collections.Generic;

namespace Imperit.Dynamics.Commands
{
    public class Repayment : ICommand
    {
        public readonly int Debtor;
        public readonly uint Amount;
        public Repayment(int debtor, uint amount)
        {
            Debtor = debtor;
            Amount = amount;
        }
        public (IAction[], State.Player) Perform(State.Player player, State.Provinces provinces)
        {
            return (System.Array.Empty<IAction>(), player.Id == Debtor ? player.Pay(Amount) : player);
        }
        public bool Allowed(IReadOnlyList<State.Player> players, State.Provinces provinces) => players[Debtor].Money >= Amount;
    }
}
