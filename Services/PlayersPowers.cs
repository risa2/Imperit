using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IPlayersPowers
    {
        void Add(IEnumerable<State.Player> players);
        void Reset();
        State.PlayerPower[][] Powers { get; }
    }
    public class PlayersPowers : IPlayersPowers
    {
        ISettingsLoader settings;
        IProvincesLoader provinces;
        IActionReader actions;
        Load.Writer<Load.PlayersPowers, State.PlayerPower[], bool> loader;
        public State.PlayerPower[][] Powers { get; private set; }
        public PlayersPowers(IServiceIO io, ISettingsLoader settings, IProvincesLoader provinces, IActionReader actions)
        {
            loader = new Load.Writer<Load.PlayersPowers, State.PlayerPower[], bool>(io.Powers, false, Load.PlayersPowers.From);
            Powers = loader.Load().ToArray();
            this.settings = settings;
            this.provinces = provinces;
            this.actions = actions;
        }
        public void Reset() => loader.Save(new State.PlayerPower[0][]);
        public void Add(IEnumerable<State.Player> players)
        {
            var debts = new uint[players.Count()];
            var soldiers = new uint[players.Count()];
            foreach (var Repayment in actions.Actions.Casted<Dynamics.Actions.Repayment, Dynamics.IAction>())
            {
                debts[Repayment.Debtor] += Repayment.Remaining;
            }
            foreach (var pa in provinces.Provinces.Select(p => p.Army).Casted<State.PlayerArmy, State.IArmy>())
            {
                soldiers[pa.Player.Id] += pa.Soldiers;
            }
            loader.Add(players.Select((p, i) => new State.PlayerPower(soldiers[i], p.Money, debts[i], p.Income)).ToArray());
        }
    }
}