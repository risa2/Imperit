using System.Linq;

namespace Imperit.Dynamics.Actions
{
    public class Instability : IAction
    {
        static readonly System.Random rand = new System.Random();
        public (IAction[], State.Province) Do(State.Province province, State.Player active)
        {
            if ((province is State.Land Land && Land.IsControlledBy(active) && !Land.IsStart && rand.NextDouble() < Land.Instability) || (province is State.Sea Sea && Sea.Soldiers == 0))
            {
                var (revolted, action) = province.Revolt();
                return (action.Append(this).ToArray(), revolted);
            }
            return (new[] { this }, province);
        }
        public byte Priority => 180;
    }
}