namespace Imperit.Dynamics.Actions
{
    public class Seizure : IAction
    {
        public readonly int Player;
        public readonly uint Amount;
        public Seizure(int player, uint amount)
        {
            Player = player;
            Amount = amount;
        }
        public (IAction[], State.Province) Perform(State.Province province, State.Player active)
        {
            if (province is State.Land land && land.IsControlledBy(Player) && Amount > 0)
            {
                var (new_province, actions) = land.Revolt();
                return (Amount <= land.Price ? actions : actions.Concat(new Seizure(Player, Amount - land.Price)), new_province);
            }
            return (new[] { this }, province);
        }
        public byte Priority => 190;
    }
}
