using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class PlayerPower
    {
        public uint Soldiers { get; set; }
        public uint Money { get; set; }
        public uint Income { get; set; }
    }
    public class PowersLoader : IConvertibleToWith<State.PlayerPower[], bool>
    {
        public IEnumerable<PlayerPower>? Powers { get; set; }
        public State.PlayerPower[] Convert(int i, bool a) => Powers.Select(p => new State.PlayerPower(p.Soldiers, p.Money, p.Income)).ToArray();
        public static PowersLoader From(State.PlayerPower[] pp) => new PowersLoader() { Powers = pp.Select(p => new PlayerPower() { Soldiers = p.Soldiers, Money = p.Money, Income = p.Income }) };
    }
}