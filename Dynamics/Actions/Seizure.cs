namespace Imperit.Dynamics.Actions
{
	public class Seizure : IAction
	{
		public readonly int Player;
		public readonly int Amount;
		public Seizure(int player, int amount)
		{
			Player = player;
			Amount = amount;
		}
		public (IAction[], State.Province) Perform(State.Province province, State.Player active)
		{
			if (province is State.Land land && land.IsControlledBy(Player) && Amount > 0)
			{
				return (Amount <= land.Price ? System.Array.Empty<IAction>() : new[] { new Seizure(Player, Amount - land.Price) }, land.Revolt());
			}
			return (new[] { this }, province);
		}
		public byte Priority => 190;
	}
}
