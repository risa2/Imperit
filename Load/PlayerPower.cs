using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.Load
{
    public class PlayerPower
    {
        public long Total { get; set; }
        public double Change { get; set; }
        public double Ratio { get; set; }
    }
    public class PowersLoader : IConvertibleToWith<ImmutableArray<State.PlayerPower>, bool>
    {
        public IEnumerable<PlayerPower>? Powers { get; set; }
        public ImmutableArray<State.PlayerPower> Convert(int i, bool a) => Powers.Select(p => new State.PlayerPower(p.Total, p.Change, p.Ratio)).ToImmutableArray();
        public static PowersLoader From(ImmutableArray<State.PlayerPower> pp) => new PowersLoader() { Powers = pp.Select(p => new PlayerPower() { Total = p.Total, Change = p.Change, Ratio = p.Ratio }) };
    }
}