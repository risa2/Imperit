using System;

namespace Imperit.State
{
	public class Game
	{
		public readonly bool IsActive, AnyRegistered;
		public readonly DateTime FirstRegistration;
		public Game(bool isActive, DateTime firstRegistration, bool anyRegistered)
		{
			IsActive = isActive;
			FirstRegistration = firstRegistration;
			AnyRegistered = anyRegistered;
		}
		public static Game Start() => new Game(true, DateTime.MaxValue, false);
		public static Game Finish() => new Game(false, DateTime.MaxValue, false);
		public Game Register() => new Game(false, AnyRegistered ? FirstRegistration : DateTime.UtcNow, true);
	}
}
