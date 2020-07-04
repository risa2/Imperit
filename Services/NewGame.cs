using System.Linq;

namespace Imperit.Services
{
    interface INewGame
    {
        void New(double interest, uint defaultMoney, uint debtLimit, double defaultInstability, bool singleClient);
        void Registration(string name, State.Password password, State.Color color, State.Land land, bool human);
        void Start();
    }
    public class NewGame : INewGame
    {
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
        public void New(double interest, uint defaultMoney, uint debtLimit, double defaultInstability, bool singleClient)
        {
            sl.Settings = new State.Settings(interest, defaultInstability, defaultMoney, debtLimit, singleClient, false);
            players.Clear();
            powers.Reset();

            pr.NewGame(sl, players);
            pr.Provinces = new State.Provinces(pr.Provinces.Select(prov => CreateProvince(prov)).ToArray(), pr.Provinces.Graph);

            pr.Save();
            writer.NewGame();
        }
        public void Start()
        {
            writer.StartGame(pr.Provinces.Casted<State.Port, State.Province>().Select(port => new Dynamics.Actions.PortRenewal(port.Id)).Concat<Dynamics.IAction>(pr.Provinces.Casted<State.Land, State.Province>().Select(land => new Dynamics.Actions.Instability(land.Id, land.Army is State.PlayerArmy pa ? pa.Player.Id as int? : null))).Concat(players.Select(pl => new Dynamics.Actions.Earn(pl.Id))).Concat<Dynamics.IAction>(players.Select(pl => new Dynamics.Actions.Mortality(pl.Id, pr.Provinces))));
            active.StartGame();
            sl.Settings = sl.Settings.Start();
            powers.Add(players);
        }
        public void Registration(string name, State.Password password, State.Color color, State.Land land, bool human)
        {
            var player = new State.Player(players.Count, name, color, isHuman: human, password, sl.Settings.DefaultMoney, credibility: 1.0, alive: true, income: land.Earnings);
            players.Add(player);
            pr.NewGame(sl, players);
            (pr.Provinces[land.Id], _) = land.GiveUpTo(new State.PlayerArmy(sl.Settings, player, land.Soldiers));
            pr.Save();
        }
    }
}