using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IPowersLoader : IReadOnlyList<State.PlayerPower[]>
    {
        void Add(IReadOnlyCollection<State.Player> players);
        void Clear();
    }
    public class PowersLoader : IPowersLoader
    {
        readonly IProvincesLoader provinces;
        readonly Load.Writer<Load.PowersLoader, State.PlayerPower[], bool> loader;
        readonly List<State.PlayerPower[]> powers;
        public int Count => powers.Count;
        public State.PlayerPower[] this[int i] => powers[i];
        public PowersLoader(IServiceIO io, IProvincesLoader provinces)
        {
            loader = new Load.Writer<Load.PowersLoader, State.PlayerPower[], bool>(io.Powers, false, Load.PowersLoader.From);
            powers = loader.Load().ToList();
            this.provinces = provinces;
        }
        public void Clear()
        {
            loader.Clear();
            powers.Clear();
        }
        public void Add(IReadOnlyCollection<State.Player> players)
        {
            var soldiers = new uint[players.Count];
            foreach (var army in provinces.Provinces.Select(p => p.Army as State.PlayerArmy).NotNull())
            {
                soldiers[army.Player.Id] += army.Soldiers;
            }
            powers.Add(players.Select((p, i) => new State.PlayerPower(soldiers[i], p.Money, p.Income)).ToArray());
            loader.Add(powers.Last());
        }
        public IEnumerator<State.PlayerPower[]> GetEnumerator() => powers.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}