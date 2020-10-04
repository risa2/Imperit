using Imperit.State;
using System.Collections.Generic;

namespace Imperit.Load
{
	public class JsonArmy : IConvertibleToWith<Army, (IReadOnlyList<Player>, IReadOnlyList<SoldierType>)>
	{
		public JsonSoldiers Soldiers { get; set; } = new JsonSoldiers();
		public int Player { get; set; }
		public Army Convert(int i, (IReadOnlyList<Player>, IReadOnlyList<SoldierType>) arg)
		{
			var (players, types) = arg;
			return new Army(Soldiers.Convert(i, types), players[Player]);
		}
		public static JsonArmy From(Army army)
		{
			return new JsonArmy() { Soldiers = JsonSoldiers.From(army.Soldiers), Player = army.PlayerId };
		}
	}
}