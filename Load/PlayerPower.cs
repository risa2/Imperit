using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class PlayerPower
    {
        public long Total { get; set; }
        public double Change { get; set; }
        public double Ratio { get; set; }
    }
    public class PowersLoader : IConvertibleToWith<State.PlayerPower[], bool>
    {
        public IEnumerable<PlayerPower>? Powers { get; set; }
        public State.PlayerPower[] Convert(int i, bool a) => Powers.Select(p => new State.PlayerPower(p.Total, p.Change, p.Ratio)).ToArray();
        public static PowersLoader From(State.PlayerPower[] pp) => new PowersLoader() { Powers = pp.Select(p => new PlayerPower() { Total = p.Total, Change = p.Change, Ratio = p.Ratio }) };
    }
}