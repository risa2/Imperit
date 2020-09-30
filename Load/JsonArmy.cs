using Imperit.State;
using System.Collections.Generic;

namespace Imperit.Load
{
	public class JsonArmy : IConvertibleToWith<Army, IReadOnlyList<Player>>
	{
		public JsonSoldiers Soldiers { get; set; } = new JsonSoldiers();
		public int Player { get; set; }
		public Army Convert(int i, IReadOnlyList<Player> players)
		{
			return new Army(Soldiers.Convert(i, players[Player].SoldierTypes), players[Player]);
		}
		public static JsonArmy From(Army army)
		{
			return new JsonArmy() { Soldiers = JsonSoldiers.From(army.Soldiers), Player = army.PlayerId };
		}
	}
}