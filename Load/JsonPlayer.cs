using Imperit.State;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.Load
{
	public class JsonPlayer : IConvertibleToWith<Player, Settings>
	{
		public string Name { get; set; } = "";
		public string Color { get; set; } = "";
		public string Type { get; set; } = "";
		public string Password { get; set; } = "";
		public int Money { get; set; }
		public bool Alive { get; set; }
		public int Income { get; set; }
		public IEnumerable<int>? SoldierTypes { get; set; }
		public Player Convert(int i, Settings settings) => Type switch
		{
			"P" => new Player(i, Name, State.Color.Parse(Color), State.Password.Parse(Password), Money, Alive, Income, SoldierTypes!.Select(i => settings.SoldierTypes[i]).ToImmutableArray()),
			"R" => new Robot(i, Name, State.Color.Parse(Color), State.Password.Parse(Password), Money, Alive, Income, SoldierTypes!.Select(i => settings.SoldierTypes[i]).ToImmutableArray()),
			"S" => new Savage(i, SoldierTypes!.Select(i => settings.SoldierTypes[i]).ToImmutableArray()),
			_ => throw new System.Exception("Unknown Player type: " + Type)
		};
		public static JsonPlayer From(Player p) => new JsonPlayer() { Name = p.Name, Color = p.Color.ToString(), Type = p is Robot ? "R" : p is Savage ? "S" : "P", Password = p.Password.ToString(), Money = p.Money, Alive = p.Alive, Income = p.Income, SoldierTypes = p.SoldierTypes.Select(t => t.Id) };
	}
}