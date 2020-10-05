using Imperit.State;

namespace Imperit.Dynamics.Actions
{
	public class Instability : IAction
	{
		static readonly System.Random rand = new System.Random();
		public (IAction?, Province) Perform(Province province, Player active)
		{
			return (this, province.Occupied && province is Land Land && Land.IsAllyOf(active.Id) && rand.NextDouble() < Land.Instability ? Land.Revolt() : province);
		}
		public byte Priority => 180;
	}
}