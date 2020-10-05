using Imperit.State;
using System;

namespace Imperit.Load
{
	public class JsonGame : IConvertibleToWith<Game, bool>
	{
		public bool IsActive { get; set; }
		public DateTime LastRegistration { get; set; }
		public Game Convert(int i, bool arg) => new Game(IsActive, LastRegistration);
		public static JsonGame From(Game game) => new JsonGame { IsActive = game.IsActive, LastRegistration = game.LastRegistration };
	}
}
