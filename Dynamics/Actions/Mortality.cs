using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics.Actions
{
    public class Mortality : IAction
    {
        public readonly int Player;
        readonly State.Provinces Provinces;
        public Mortality(int player, State.Provinces provinces)
        {
            Player = player;
            Provinces = provinces;
        }
        public IAction Do(IArray<State.Player> players, State.Provinces provinces, int active)
        {
            if (players[Player].Surviving)
            {
                return this;
            }
            else
            {
                players[Player] = players[Player].Die();
                var result = new List<IAction>();
                foreach ((int i, var province) in Provinces.Enumerate().Where(province => province.Item2.IsControlledBy(players[Player])))
                {
                    (var prov, var action) = province.GiveUpTo(province.DefaultArmy);
                    provinces[i] = prov;
                    result.Add(action.Do(players, provinces, active));
                }
                return new Combination(result);
            }
        }
        public int Priority => 800;
    }
}