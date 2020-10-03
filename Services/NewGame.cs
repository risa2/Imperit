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
		void Registration(string name, Password password, Color color, Land land);
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

			players.Add(new Savage(0, settings.DefaultSoldierTypes));
			provinces.Reset(sl.Settings, players);
			provinces.Set(provinces.Select(prov => CreateProvince(prov, players[0])).ToArray());
			provinces.Save();
		}
		Province[] UnoccupiedStartLands() => provinces.Where(p => p is Land L1 && L1.IsStart && !L1.Occupied).ToArray();
		void AddRobots()
		{
			var start_lands = UnoccupiedStartLands();
			rand.Shuffle(start_lands);
			int count = Math.Min(start_lands.Length, sl.Settings.MaxRobotCount), previous = players.Count;
			var colors = rand.NextColors(count);
			for (int i = 0; i < count; ++i)
			{
				players.Add(new Robot(players.Count, sl.Settings.RobotName(i), colors[i], new Password(""), sl.Settings.DefaultMoney, true, sl.Settings.DefaultSoldierTypes));
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
		public void Registration(string name, Password password, Color color, Land land)
		{
			var player = new Player(players.Count, name, color, password, sl.Settings.DefaultMoney, true, sl.Settings.DefaultSoldierTypes);
			players.Add(player);
			provinces.Reset(sl.Settings, players);
			provinces[land.Id] = land.GiveUpTo(new Army(land.Soldiers, player));
			provinces.Save();
		}
	}
}