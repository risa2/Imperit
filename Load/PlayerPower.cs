using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
    public class PlayerPower
    {
        public uint Soldiers { get; set; }
        public uint Money { get; set; }
        public uint Debt { get; set; }
        public uint Income { get; set; }
    }
    public class PlayersPowers : IConvertibleToWith<State.PlayerPower[], bool>
    {
        public IEnumerable<PlayerPower>? Powers { get; set; }
        public State.PlayerPower[] Convert(int i, bool a) => Powers.Select(p => new State.PlayerPower(p.Soldiers, p.Money, p.Debt, p.Income)).ToArray();
        public static PlayersPowers From(State.PlayerPower[] pp) => new PlayersPowers() { Powers = pp.Select(p => new PlayerPower() { Soldiers = p.Soldiers, Money = p.Money, Debt = p.Debt, Income = p.Income }) };
    }
}