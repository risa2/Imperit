namespace Imperit.State
{
    public class PlayerArmy : IArmy
    {
        readonly Settings settings;
        public readonly Player Player;
        public uint Soldiers { get; }
        public PlayerArmy(Settings set, Player player, uint soldiers)
        {
            settings = set;
            Player = player;
            Soldiers = soldiers;
        }
        public double Hostility => settings.Instability(Soldiers);
        public Dynamics.IAction Gain(Province what) => new Dynamics.Actions.IncomeChange(Player.Id, (int)what.Earnings);
        public Dynamics.IAction Lose(Province what) => new Dynamics.Actions.IncomeChange(Player.Id, -(int)what.Earnings);
        public IArmy Join(IArmy another) => new PlayerArmy(settings, Player, Soldiers + another.Soldiers);
        public IArmy Subtract(IArmy another) => new PlayerArmy(settings, Player, Soldiers - another.Soldiers);
        public bool IsControlledBy(int player) => Player.Id == player;
        public bool IsAllyOf(IArmy another) => another.IsControlledBy(Player.Id);
    }
}