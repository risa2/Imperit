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
            (var settings, var players) = arg;
            switch (Type)
            {
                case "Peasant":
                    return new State.PeasantArmy(Soldiers);
                case "Player":
                    return new State.PlayerArmy(settings, players[Player], Soldiers);
                default:
                    throw new System.Exception("Unknown Army type: " + Type);
            }
        }
        public static Army FromArmy(State.IArmy value)
        {
            if (value is State.PeasantArmy Peasant)
                return new Army() { Type = "Peasant", Soldiers = Peasant.Soldiers };
            if (value is State.PlayerArmy Player)
                return new Army() { Type = "Player", Soldiers = Player.Soldiers, Player = Player.Player.Id };
            throw new System.Exception("Invalid type of IArmy " + value);
        }
    }
}