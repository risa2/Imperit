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
        readonly IProvincesLoader pr;
        readonly IActionWriter actions;
        readonly IActivePlayer active;
        readonly IPlayersPowers powers;
        public NewGame(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader pr, IActionWriter actions, IActivePlayer active, IPlayersPowers powers)
        {
            this.sl = sl;
            this.players = players;
            this.pr = pr;
            this.actions = actions;
            this.active = active;
            this.powers = powers;
        }
        static State.Province CreateProvince(State.Province p)
        {
            (var province, _) = p.GiveUpTo(p.DefaultArmy);
            return province is State.Port Port ? Port.Renew() : province;
        }
        public void New(double interest, uint defaultMoney, uint debtLimit, double defaultInstability, bool singleClient, string[] robotNames, int maxRobotCount)
        {
            sl.Settings = new State.Settings(interest, defaultInstability, defaultMoney, debtLimit, robotNames, maxRobotCount, singleClient, false);
            players.Clear();
            powers.Clear();
            actions.Clear();

            pr.Reset(sl.Settings, players);
            pr.Set(pr.Provinces.Select(prov => CreateProvince(prov)).ToArray());
            pr.Save();
        }
        State.Province[] UnoccupiedStartLands() => pr.Provinces.Where(p => p is State.Land L1 && L1.IsStart && !L1.Occupied).ToArray();
        void AddRobots()
        {
            var start_lands = UnoccupiedStartLands();
            rand.Shuffle(start_lands);
            int count = Math.Min(start_lands.Length, sl.Settings.MaxRobotCount), previous = players.Count;
            for (int i = 0; i < count; ++i)
            {
                players.Add(new State.Robot(players.Count, sl.Settings.RobotName(i), rand.NextColor(), State.Password.FromString(""), sl.Settings.DefaultMoney, 1.0, true, start_lands[i].Earnings));
            }
            pr.Reset(sl.Settings, players);
            for (int i = 0; i < count; ++i)
            {
                (pr.Provinces[start_lands[i].Id], _) = start_lands[i].GiveUpTo(new State.PlayerArmy(sl.Settings, players[previous + i], start_lands[i].Soldiers));
            }
            pr.Save();
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
            var player = new State.Player(players.Count, name, color, password, sl.Settings.DefaultMoney, credibility: 1.0, alive: true, income: land.Earnings);
            players.Add(player);
            pr.Reset(sl.Settings, players);
            (pr.Provinces[land.Id], _) = land.GiveUpTo(new State.PlayerArmy(sl.Settings, player, land.Soldiers));
            pr.Save();
        }
    }
}