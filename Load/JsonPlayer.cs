using Imperit.State;

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
		public Player Convert(int i, Settings settings) => Type switch
		{
			"P" => new Player(i, Name, State.Color.Parse(Color), State.Password.Parse(Password), Money, Alive),
			"R" => new Robot(i, Name, State.Color.Parse(Color), State.Password.Parse(Password), Money, Alive, settings),
			"S" => new Savage(i),
			_ => throw new System.Exception("Unknown Player type: " + Type)
		};
		public static JsonPlayer From(Player p) => new JsonPlayer { Name = p.Name, Color = p.Color.ToString(), Type = p is Robot ? "R" : p is Savage ? "S" : "P", Password = p.Password.ToString(), Money = p.Money, Alive = p.Alive };
	}
}