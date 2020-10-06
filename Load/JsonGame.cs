using System;
using Imperit.State;

namespace Imperit.Load
{
	public class JsonGame : IConvertibleToWith<Game, Settings>
	{
		public bool IsActive { get; set; }
		public DateTime FirstRegistration { get; set; }
		public bool AnyRegistered { get; set; }
		public Game Convert(int i, Settings s) => new Game(IsActive, FirstRegistration, AnyRegistered);
		public static JsonGame From(Game game) => new JsonGame { IsActive = game.IsActive, FirstRegistration = game.FirstRegistration, AnyRegistered = game.AnyRegistered };
	}
}
