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
        uint[] SoldiersOf(IReadOnlyCollection<State.Player> players)
        {
            var soldiers = new uint[players.Count];
            foreach (var army in provinces.Select(p => p.Army as State.PlayerArmy).NotNull())
            {
                soldiers[army.Player.Id] += army.Soldiers;
            }
            return soldiers;
        }
        static double Div(double a, double b) => (a / b) switch
        {
            double.NaN => 0.0,
            double.PositiveInfinity => double.MaxValue,
            double.NegativeInfinity => double.MinValue,
            _ => a / b
        };
        public void Add(IReadOnlyCollection<State.Player> players)
        {
            var soldiers = SoldiersOf(players);
            var totals = players.Select((p, i) => (long)soldiers[i] + p.Money + (p.Income * 5)).ToArray();
            var changes = players.Select((p, i) => powers.Any() ? Div(totals[i], powers.Last()[i].Total) - 1 : 0.0).ToArray();
            var sum_sm = players.Select((p, i) => (long)soldiers[i] + p.Money).Sum();
            var ratios = players.Select((p, i) => Div(soldiers[i] + p.Money, sum_sm)).ToArray();
            powers.Add(players.Select((p, i) => new State.PlayerPower(totals[i], changes[i], ratios[i])).ToArray());
            loader.Add(powers.Last());
        }
        public IEnumerator<State.PlayerPower[]> GetEnumerator() => powers.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}