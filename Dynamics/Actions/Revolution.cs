namespace Imperit.Dynamics.Actions
{
    public class Revolution : IAction
    {
        public readonly int Province;
        public Revolution(int province) => Province = province;
        public (IAction[], State.Province) Perform(State.Province province, State.Player active) => province.Revolt().Swap();
        public byte Priority => 0;
    }
}
