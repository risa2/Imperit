namespace Imperit.Dynamics.Commands
{
    public class Attack : Move
    {
        public Attack(int player, int from, State.Province to, State.IArmy army) : base(player, from, to, army) { }
        protected override Actions.ArmyOperation GetMove() => new Actions.Attack(To.Id, Army);
    }
}