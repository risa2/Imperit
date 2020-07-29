namespace Imperit.State
{
    public class PlayerArmy : IArmy
    {
        public readonly Player Player;
        public uint Soldiers { get; }
        public Color? Color => Player.Color;
        public PlayerArmy(Player player, uint soldiers)
        {
            Player = player;
            Soldiers = soldiers;
        }
        public Dynamics.IAction Gain(Province what) => new Dynamics.Actions.IncomeChange(Player.Id, (int)what.Earnings);
        public Dynamics.IAction Lose(Province what) => new Dynamics.Actions.IncomeChange(Player.Id, -(int)what.Earnings);
        public IArmy Join(IArmy another) => new PlayerArmy(Player, Soldiers + another.Soldiers);
        public IArmy Subtract(IArmy another) => new PlayerArmy(Player, Soldiers - another.Soldiers);
        public bool IsControlledBy(int player) => Player.Id == player;
        public bool IsAllyOf(IArmy another) => another.IsControlledBy(Player.Id);
    }
}