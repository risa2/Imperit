using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.Services
{
    public interface IPowersLoader : IReadOnlyList<ImmutableArray<State.PlayerPower>>
    {
        void Add(IReadOnlyCollection<State.Player> players);
        void Clear();
    }
    public class PowersLoader : IPowersLoader
    {
        readonly IProvincesLoader provinces;
        readonly Load.Writer<Load.PowersLoader, ImmutableArray<State.PlayerPower>, bool> loader;
        readonly List<ImmutableArray<State.PlayerPower>> powers;
        public int Count => powers.Count;
        public ImmutableArray<State.PlayerPower> this[int i] => powers[i];
        public PowersLoader(IServiceIO io, IProvincesLoader provinces)
        {
            loader = new Load.Writer<Load.PowersLoader, ImmutableArray<State.PlayerPower>, bool>(io.Powers, false, Load.PowersLoader.From);
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
            return players.Select(player => (uint)provinces.Where(prov => prov.IsControlledBy(player.Id))
                                                           .Sum(prov => prov.Soldiers)).ToArray();
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
            powers.Add(players.Select((p, i) => new State.PlayerPower(totals[i], changes[i], ratios[i])).ToImmutableArray());
            loader.Add(powers.Last());
        }
        public IEnumerator<ImmutableArray<State.PlayerPower>> GetEnumerator() => powers.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}