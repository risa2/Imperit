using System;

namespace Imperit.State
{
	public class Game
	{
		public readonly bool IsActive;
		public readonly DateTime LastRegistration;
		public Game(bool isActive, DateTime lastRegistration)
		{
			IsActive = isActive;
			LastRegistration = lastRegistration;
		}
		public Game Start() => new Game(true, LastRegistration);
		public Game Finish() => new Game(false, LastRegistration);
		public Game Register(DateTime when) => new Game(IsActive, when);
	}
}
