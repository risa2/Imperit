using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IPlayersPowers
    {
        State.PlayerPower[][] Powers { get; }
        void Add(IEnumerable<State.Player> players);
        void Clear();
    }
    public class PlayersPowers : IPlayersPowers
    {
        readonly IProvincesLoader provinces;
        readonly Load.Writer<Load.PlayersPowers, State.PlayerPower[], bool> loader;
        public State.PlayerPower[][] Powers { get; private set; }
        public PlayersPowers(IServiceIO io, IProvincesLoader provinces)
        {
            loader = new Load.Writer<Load.PlayersPowers, State.PlayerPower[], bool>(io.Powers, false, Load.PlayersPowers.From);
            Powers = loader.Load().ToArray();
            this.provinces = provinces;
        }
        public void Clear() => loader.Save(new State.PlayerPower[0][]);
        public void Add(IEnumerable<State.Player> players)
        {
            var soldiers = new uint[players.Count()];
            foreach (var army in provinces.Provinces.Select(p => p.Army as State.PlayerArmy).NotNull())
            {
                soldiers[army.Player.Id] += army.Soldiers;
            }
            loader.Add(players.Select((p, i) => new State.PlayerPower(soldiers[i], p.Money, p.Income)).ToArray());
        }
    }
}