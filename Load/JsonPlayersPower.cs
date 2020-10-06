using Imperit.State;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.Load
{
	public class JsonPlayersPower : IConvertibleToWith<PlayersPower, bool>
	{
		public IEnumerable<JsonPlayerPower>? Powers { get; set; }
		public PlayersPower Convert(int i, bool a) => new PlayersPower(Powers.Select(p => new PlayerPower(p.Alive, p.Soldiers, p.Lands, p.Income, p.Money)).ToImmutableArray());
		public static JsonPlayersPower From(PlayersPower pp) => new JsonPlayersPower { Powers = pp.Select(p => new JsonPlayerPower { Alive = p.Alive, Soldiers = p.Soldiers, Lands = p.Lands, Income = p.Income, Money = p.Money }) };
	}
}