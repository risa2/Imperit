using Imperit.Dynamics;
using Imperit.Dynamics.Actions;
using Imperit.State;
using System;
using System.Linq;

namespace Imperit.Services
{
	interface INewGame
	{
		void New(Settings settings);
		Color NextColor { get; }
		void Registration(string name, Password password, Land land);
		void Start();
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
		public NewGame(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader provinces, IActionLoader actions, IActivePlayer active, IPowersLoader powers, ILoginSession login)
		{
			this.sl = sl;
			this.players = players;
			this.provinces = provinces;
			this.actions = actions;
			this.active = active;
			this.powers = powers;
			this.login = login;
		}
		static Province CreateProvince(Province province, Player savage)
		{
			return province.GiveUpTo(new Army(province.DefaultSoldiers, savage));
		}
		public void New(Settings settings)
		{
			sl.Settings = settings;
			players.Clear();
			powers.Clear();
			actions.Clear();
			login.Clear();

			players.Add(new Savage(0));
			provinces.Reset(sl.Settings, players);
			provinces.Set(provinces.Select(prov => CreateProvince(prov, players[0])).ToArray());
			provinces.Save();
		}
		public Color NextColor => new Color(120.0 + (137.507764050037854 * (players.Count - 1)), 1.0, 1.0);
		Province[] UnoccupiedStartLands() => provinces.Where(p => p is Land L1 && L1.IsStart && !L1.Occupied).ToArray();
		void AddRobots()
		{
			var start_lands = UnoccupiedStartLands();
			rand.Shuffle(start_lands);
			int count = Math.Min(start_lands.Length, sl.Settings.MaxRobotCount), previous = players.Count;
			for (int i = 0; i < count; ++i)
			{
				players.Add(new Robot(players.Count, sl.Settings.RobotName(i), NextColor, new Password(""), sl.Settings.DefaultMoney, true, sl.Settings));
			}
			provinces.Reset(sl.Settings, players);
			for (int i = 0; i < count; ++i)
			{
				provinces[start_lands[i].Id] = start_lands[i].GiveUpTo(new Army(start_lands[i].Soldiers, players[previous + i]));
			}
			provinces.Save();
		}
		public void Start()
		{
			AddRobots();
			actions.Save(new ActionQueue(new[] { new Instability() as IAction, new Earnings(), new Mortality() }));
			active.Reset(players);
			sl.Settings = sl.Settings.Start();
			powers.Add(players);
		}
		public void Registration(string name, Password password, Land land)
		{
			var player = new Player(players.Count, name, NextColor, password, sl.Settings.DefaultMoney, true);
			players.Add(player);
			provinces.Reset(sl.Settings, players);
			provinces[land.Id] = land.GiveUpTo(new Army(land.Soldiers, player));
			provinces.Save();
		}
	}
}