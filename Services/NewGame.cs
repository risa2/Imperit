using System;
using System.Linq;

namespace Imperit.Services
{
    interface INewGame
    {
        void New(double interest, uint defaultMoney, uint debtLimit, double defaultInstability, bool singleClient, string[] robotNames, int maxRobotCount);
        void Registration(string name, State.Password password, State.Color color, State.Land land);
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
        public NewGame(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader provinces, IActionLoader actions, IActivePlayer active, IPowersLoader powers)
        {
            this.sl = sl;
            this.players = players;
            this.provinces = provinces;
            this.actions = actions;
            this.active = active;
            this.powers = powers;
        }
        static State.Province CreateProvince(State.Province p)
        {
            var (province, _) = p.GiveUpTo(p.DefaultArmy);
            return province is State.Port Port ? Port.Renew() : province;
        }
        public void New(double interest, uint defaultMoney, uint debtLimit, double defaultInstability, bool singleClient, string[] robotNames, int maxRobotCount)
        {
            sl.Settings = new State.Settings(interest, defaultInstability, defaultMoney, debtLimit, robotNames, maxRobotCount, singleClient, false);
            players.Clear();
            powers.Clear();
            actions.Clear();

            provinces.Reset(sl.Settings, players);
            provinces.Set(provinces.Select(prov => CreateProvince(prov)).ToArray());
            provinces.Save();
        }
        State.Province[] UnoccupiedStartLands() => provinces.Where(p => p is State.Land L1 && L1.IsStart && !L1.Occupied).ToArray();
        void AddRobots()
        {
            var start_lands = UnoccupiedStartLands();
            rand.Shuffle(start_lands);
            int count = Math.Min(start_lands.Length, sl.Settings.MaxRobotCount), previous = players.Count;
            var colors = rand.NextColors(count);
            for (int i = 0; i < count; ++i)
            {
                players.Add(new State.Robot(players.Count, sl.Settings.RobotName(i), colors[i], State.Password.FromString(""), sl.Settings.DefaultMoney, true, start_lands[i].Earnings));
            }
            provinces.Reset(sl.Settings, players);
            for (int i = 0; i < count; ++i)
            {
                (provinces[start_lands[i].Id], _) = start_lands[i].GiveUpTo(new State.PlayerArmy(players[previous + i], start_lands[i].Soldiers));
            }
            provinces.Save();
        }
        public void Start()
        {
            AddRobots();
            actions.Save(new Dynamics.ActionQueue(new[] { new Dynamics.Actions.PortRenewal() as Dynamics.IAction, new Dynamics.Actions.Instability(), new Dynamics.Actions.Earn(), new Dynamics.Actions.Mortality() }));
            active.Reset();
            sl.Settings = sl.Settings.Start();
            powers.Add(players);
        }
        public void Registration(string name, State.Password password, State.Color color, State.Land land)
        {
            var player = new State.Player(players.Count, name, color, password, sl.Settings.DefaultMoney, alive: true, income: land.Earnings);
            players.Add(player);
            provinces.Reset(sl.Settings, players);
            (provinces[land.Id], _) = land.GiveUpTo(new State.PlayerArmy(player, land.Soldiers));
            provinces.Save();
        }
    }
}