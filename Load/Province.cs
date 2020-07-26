using System.Collections.Generic;

namespace Imperit.Load
{
    public class Province : IConvertibleToWith<State.Province, (State.Settings, IReadOnlyList<State.Player>, IReadOnlyList<State.Shape>)>
    {
        public string? Type { get; set; }
        public string? Name { get; set; }
        public Army? Army { get; set; }
        public Army? DefaultArmy { get; set; }
        public uint? Earnings { get; set; }
        public uint? CanBoard { get; set; }
        public uint? Capacity { get; set; }
        public bool? IsStart { get; set; }
        public State.Province Convert(int i, (State.Settings, IReadOnlyList<State.Player>, IReadOnlyList<State.Shape>) arg)
        {
            var (settings, players, shapes) = arg;
            return Type switch
            {
                "S" => new State.Sea(i, Name!, shapes[i], Army!.Convert(i, (settings, players))),
                "L" => new State.Land(i, Name!, shapes[i], Army!.Convert(i, (settings, players)), DefaultArmy!.Convert(i, (settings, players)), IsStart ?? true, Earnings ?? 0),
                "P" => new State.Port(i, Name!, shapes[i], Army!.Convert(i, (settings, players)), DefaultArmy!.Convert(i, (settings, players)), IsStart ?? true, Earnings ?? 0, Capacity ?? 0, CanBoard ?? 0),
                _ => throw new System.Exception("Unknown State.Province type: " + Type)
            };
        }
        public static Province FromProvince(State.Province value)
        {
            return value switch
            {
                State.Port Port => new Province() { Type = "P", Name = Port.Name, Army = Army.FromArmy(Port.Army), IsStart = Port.IsStart, Earnings = Port.Earnings, DefaultArmy = Army.FromArmy(Port.DefaultArmy), Capacity = Port.Capacity, CanBoard = Port.CanBoard },
                State.Land Land => new Province() { Type = "L", Name = Land.Name, Army = Army.FromArmy(Land.Army), IsStart = Land.IsStart, Earnings = Land.Earnings, DefaultArmy = Army.FromArmy(Land.DefaultArmy) },
                State.Sea Sea => new Province() { Type = "S", Name = Sea.Name, Army = Army.FromArmy(Sea.Army) },
                _ => throw new System.Exception("Unknown type of Province")
            };
        }
    }
}