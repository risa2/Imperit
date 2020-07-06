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
        readonly IActionWriter writer;
        readonly IActivePlayer active;
        readonly IPlayersPowers powers;
        public NewGame(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader pr, IActionWriter writer, IActivePlayer active, IPlayersPowers powers)
        {
            this.sl = sl;
            this.players = players;
            this.pr = pr;
            this.writer = writer;
            this.active = active;
            this.powers = powers;
        }
        static State.Province CreateProvince(State.Province p)
        {
            var province = p.GiveUpTo(p.DefaultArmy).Item1;
            return province is State.Port Port ? Port.Renew() : province;
        }
        public void New(double interest, uint defaultMoney, uint debtLimit, double defaultInstability, bool singleClient, string[] robotNames, int maxRobotCount)
        {
            sl.Settings = new State.Settings(interest, defaultInstability, defaultMoney, debtLimit, singleClient, false, robotNames, maxRobotCount);
            players.Clear();
            powers.Reset();

            pr.Reset(sl.Settings, players);
            pr.Provinces = new State.Provinces(pr.Provinces.Select(prov => CreateProvince(prov)).ToArray(), pr.Provinces.Graph);

            pr.Save();
            writer.Clear();
        }
        public void Start()
        {
            var freelands = pr.Provinces.Where(p => p is State.Land L1 && L1.IsStart && !L1.Occupied).Select(p => p.Id).ToArray();
            int added_count = Math.Min(freelands.Length, sl.Settings.MaxRobotCount), prev_count = players.Count;
            for (int i = 0; i < added_count; ++i)
            {
                players.Add(new State.Player(players.Count, sl.Settings.RobotName(i), rand.NextColor(), false, State.Password.FromString(""), sl.Settings.DefaultMoney, 1.0, true, pr.Provinces[freelands[i]].Earnings));
            }
            pr.Reset(sl.Settings, players);
            for (int i = 0; i < added_count; ++i)
            {
                (pr.Provinces[freelands[i]], _) = pr.Provinces[freelands[i]].GiveUpTo(new State.PlayerArmy(sl.Settings, players[prev_count + i], pr.Provinces[freelands[i]].Soldiers));
            }
            pr.Save();
            writer.Save(new Dynamics.ActionQueue(pr.Provinces.Casted<State.Port, State.Province>().Select(port => new Dynamics.Actions.PortRenewal(port.Id)).Concat<Dynamics.IAction>(pr.Provinces.Casted<State.Land, State.Province>().Select(land => new Dynamics.Actions.Instability(land.Id, land.Army is State.PlayerArmy pa ? pa.Player.Id as int? : null))).Concat(players.Select(pl => new Dynamics.Actions.Earn(pl.Id))).Concat<Dynamics.IAction>(players.Select(pl => new Dynamics.Actions.Mortality(pl.Id, pr.Provinces))).ToList()));
            active.Reset();
            sl.Settings = sl.Settings.Start();
            powers.Add(players);
        }
        public void Registration(string name, State.Password password, State.Color color, State.Land land)
        {
            var player = new State.Player(players.Count, name, color, isHuman: true, password, sl.Settings.DefaultMoney, credibility: 1.0, alive: true, income: land.Earnings);
            players.Add(player);
            pr.Reset(sl.Settings, players);
            (pr.Provinces[land.Id], _) = land.GiveUpTo(new State.PlayerArmy(sl.Settings, player, land.Soldiers));
            pr.Save();
        }
    }
}