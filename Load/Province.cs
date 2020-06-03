using System.Collections.Generic;

namespace Imperit.Load
{
    public class Province : IConvertibleToWith<State.Province, (State.Settings, IReadOnlyList<State.Player>)>
    {
        public string? Type { get; set; }
        public string? Name { get; set; }
        public Army? Army { get; set; }
        public Army? DefaultArmy { get; set; }
        public uint? Earnings { get; set; }
        public uint? CanBoard { get; set; }
        public uint? Capacity { get; set; }
        public bool? IsStart { get; set; }
        public State.Province Convert(int i, (State.Settings, IReadOnlyList<State.Player>) arg)
        {
            (var settings, var players) = arg;
            if (Type == "S")
                return new State.Sea(i, name: Name!, army: Army!.Convert(i, arg));
            if (Type == "L")
                return new State.Land(i, name: Name!, army: Army!.Convert(i, arg), isStart: IsStart ?? true, earnings: Earnings ?? 0, defaultArmy: DefaultArmy!.Convert(i, arg));
            if (Type == "P")
                return new State.Port(i, name: Name!, army: Army!.Convert(i, arg), isStart: IsStart ?? true, earnings: Earnings ?? 0, defaultArmy: DefaultArmy!.Convert(i, arg), capacity: Capacity ?? 0, boardLimit: CanBoard ?? 0);
            throw new System.Exception("Unknown State.Province type: " + Type);
        }
        public static Province FromProvince(State.Province value)
        {
            if (value is State.Port Port)
                return new Province() { Type = "P", Name = Port.Name, Army = Army.FromArmy(Port.Army), IsStart = Port.IsStart, Earnings = Port.Earnings, DefaultArmy = Army.FromArmy(Port.DefaultArmy), Capacity = Port.Capacity, CanBoard = Port.CanBoard };
            if (value is State.Land Land)
                return new Province() { Type = "L", Name = Land.Name, Army = Army.FromArmy(Land.Army), IsStart = Land.IsStart, Earnings = Land.Earnings, DefaultArmy = Army.FromArmy(Land.DefaultArmy) };
            if (value is State.Sea Sea)
                return new Province() { Type = "S", Name = Sea.Name, Army = Army.FromArmy(Sea.Army) };
            throw new System.Exception("Unknown type of Province");
        }
    }
}