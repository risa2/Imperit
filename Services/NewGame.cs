using Imperit.Dynamics;
using Imperit.Dynamics.Actions;
using Imperit.State;
using System;
using System.Linq;

namespace Imperit.Services
{
	public interface INewGame
	{
		void Finish();
		void Start();
		Color NextColor { get; }
		void Registration(string name, Password password, Land land);
	}
	public class NewGame : INewGame
	{
		static readonly Random rand = new Random();
		readonly ISettingsLoader sl;
		readonly IPlayersLoader players;
		readonly IProvincesLoader provinces;
		readonly IActionLoader actions;
		readonly IActivePlayer active;
		readonly IPowersLoader powers;
		readonly ILoginSession login;
		readonly IGameLoader game;
		public NewGame(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader provinces, IActionLoader actions, IActivePlayer active, IPowersLoader powers, ILoginSession login, IGameLoader game)
		{
			this.sl = sl;
			this.players = players;
			this.provinces = provinces;
			this.actions = actions;
			this.active = active;
			this.powers = powers;
			this.login = login;
			this.game = game;
		}
		public void Finish()
		{
			actions.Save(new ActionQueue(new[] { new Instability() as IAction, new Earnings(), new Mortality() }));
			players.Clear();
			players.Add(new Savage(0));
			login.Clear();
			game.Finish();

			provinces.Reset(sl.Settings, players);
			provinces.Set(provinces.Select(prov => prov.Revolt()).ToArray());
			provinces.Save();
		}
		public Color NextColor => new Color(120.0 + (137.507764050037854 * (players.Count - 1)), 1.0, 1.0);
		Province[] UnoccupiedStartLands() => provinces.Where(p => p is Land L1 && L1.IsStart && !L1.Occupied).ToArray();
		void AddRobots()
		{
			var start_lands = UnoccupiedStartLands();
			rand.Shuffle(start_lands);
			for (int i = 0; i < start_lands.Length && i < sl.Settings.RobotNames.Length; ++i)
			{
				players.Add(new Robot(players.Count, sl.Settings.RobotNames[i], NextColor, new Password(""), sl.Settings.DefaultMoney, true, sl.Settings));
				provinces[start_lands[i].Id] = start_lands[i].GiveUpTo(new Army(start_lands[i].Soldiers, players[^1]));
			}
			provinces.Reset(sl.Settings, players);
			provinces.Save();
		}
		public void Start()
		{
			AddRobots();
			powers.Clear();
			game.Start();

			active.Reset(players);
			powers.Add(players);
		}
		public void Registration(string name, Password password, Land land)
		{
			var player = new Player(players.Count, name, NextColor, password, sl.Settings.DefaultMoney - (land.Earnings * 2), true);
			players.Add(player);

			provinces.Reset(sl.Settings, players);
			provinces[land.Id] = land.GiveUpTo(new Army(land.Soldiers, player));
			provinces.Save();
			game.Register();
		}
	}
}