namespace Imperit.Dynamics.Actions
{
    public class Instability : IAction
    {
        static readonly System.Random rand = new System.Random();
        public (IAction? NewThis, IAction[] Side, State.Province) Do(State.Province province, State.Player active)
        {
            if ((province is State.Land Land && Land.IsControlledBy(active) && !Land.IsStart && rand.NextDouble() < Land.Instability) || (province is State.Sea Sea && Sea.Soldiers == 0))
            {
                (var revolted, var action) = province.Revolt();
                return (this, action, revolted);
            }
            return (this, new IAction[0], province);
        }
        public byte Priority => 180;
    }
}