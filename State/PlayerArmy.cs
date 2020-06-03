namespace Imperit.State
{
    public class PlayerArmy : IArmy
    {
        private readonly Settings settings;
        public readonly Player Player;
        public uint Soldiers { get; }
        public PlayerArmy(Settings set, Player player, uint soldiers)
        {
            settings = set;
            Player = player;
            Soldiers = soldiers;
        }
        public double Hostility => settings.Instability(Soldiers, Player.Credibility);
        public Dynamics.IAction Gain(Province what) => new Dynamics.Actions.IncomeIncrease(Player.Id, what.Earnings);
        public Dynamics.IAction Lose(Province what) => new Dynamics.Actions.IncomeDecrease(Player.Id, what.Earnings);
        public IArmy Join(IArmy another) => new PlayerArmy(settings, Player, Soldiers + another.Soldiers);
        public IArmy Subtract(IArmy another) => new PlayerArmy(settings, Player, Soldiers - another.Soldiers);
        public bool IsControlledBy(Player player) => Player == player;
        public bool IsAllyOf(IArmy another) => another.IsControlledBy(Player);
    }
}