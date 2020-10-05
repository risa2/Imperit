using Imperit.State;

namespace Imperit.Dynamics.Actions
{
	public class Instability : IAction
	{
		static readonly System.Random rand = new System.Random();
		public (IAction[], Province) Perform(Province province, Player active)
		{
			if (province.Occupied && province is Land Land && Land.IsAllyOf(active.Id) && rand.NextDouble() < Land.Instability)
			{
				return (new[] { this }, Land.Revolt());
			}
			return (new[] { this }, province);
		}
		public byte Priority => 180;
	}
}