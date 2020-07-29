using System.Collections.Generic;

namespace Imperit.Load
{
    public class Army : IConvertibleToWith<State.IArmy, (State.Settings, IReadOnlyList<State.Player>)>
    {
        public string? Type { get; set; }
        public uint Soldiers { get; set; }
        public int Player { get; set; }
        public State.IArmy Convert(int i, (State.Settings, IReadOnlyList<State.Player>) arg)
        {
            var (_, players) = arg;
            return Type switch
            {
                "Peasant" => new State.PeasantArmy(Soldiers),
                "Player" => new State.PlayerArmy(players[Player], Soldiers),
                _ => throw new System.Exception("Unknown Army type: " + Type),
            };
        }
        public static Army FromArmy(State.IArmy value)
        {
            return value switch
            {
                State.PeasantArmy Peasant => new Army() { Type = "Peasant", Soldiers = Peasant.Soldiers },
                State.PlayerArmy Player => new Army() { Type = "Player", Soldiers = Player.Soldiers, Player = Player.Player.Id },
                _ => throw new System.Exception("Invalid type of IArmy " + value)
            };
        }
    }
}