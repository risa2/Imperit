namespace Imperit.Dynamics.Actions
{
	public class Instability : IAction
	{
		static readonly System.Random rand = new System.Random();
		public (IAction[], State.Province) Perform(State.Province province, State.Player active)
		{
			if (province.Occupied && province is State.Land Land && Land.IsControlledBy(active.Id) && rand.NextDouble() < Land.Instability)
			{
				return (new[] { this }, Land.Revolt());
			}
			return (new[] { this }, province);
		}
		public byte Priority => 180;
	}
}