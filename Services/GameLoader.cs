using Imperit.Load;
using Imperit.State;
using System;

namespace Imperit.Services
{
	public interface IGameLoader
	{
		void Register();
		bool Started { get; set; }
		TimeSpan TimeSinceLastRegistration { get; }
	}
	public class GameLoader : IGameLoader
	{
		readonly JsonWriter<JsonGame, Game, bool> writer;
		Game game;
		public GameLoader(IServiceIO io)
		{
			writer = new JsonWriter<JsonGame, Game, bool>(io.Game, false, JsonGame.From);
			game = writer.LoadOne();
		}
		public bool Started
		{
			get => game.IsActive;
			set
			{
				game = (value ? game.Start() : game.Finish()).Register(DateTime.MinValue);
				writer.Save(game);
			}
		}
		public TimeSpan TimeSinceLastRegistration => DateTime.UtcNow.Subtract(game.LastRegistration);
		public void Register()
		{
			game = game.Register(DateTime.UtcNow);
			writer.Save(game);
		}
	}
}
