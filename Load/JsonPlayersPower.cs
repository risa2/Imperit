using Imperit.State;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.Load
{
    public class JsonPlayersPower : IConvertibleToWith<ImmutableArray<PlayerPower>, bool>
    {
        public IEnumerable<JsonPlayerPower>? Powers { get; set; }
        public ImmutableArray<PlayerPower> Convert(int i, bool a) => Powers.Select(p => new PlayerPower(p.Total, p.Change, p.Ratio)).ToImmutableArray();
        public static JsonPlayersPower From(ImmutableArray<PlayerPower> pp) => new JsonPlayersPower() { Powers = pp.Select(p => new JsonPlayerPower() { Total = p.Total, Change = p.Change, Ratio = p.Ratio }) };
    }
}