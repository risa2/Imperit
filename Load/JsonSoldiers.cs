using Imperit.State;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Load
{
	public class JsonSoldiers : IConvertibleToWith<Soldiers, IReadOnlyList<SoldierType>>
	{
		public class Pair
		{
			public int Type { get; set; }
			public int Count { get; set; }
		}
		public IEnumerable<Pair> Items { get; set; } = System.Array.Empty<Pair>();
		public Soldiers Convert(int i, IReadOnlyList<SoldierType> types)
		{
			return new Soldiers(Items.Select(item => (types[item.Type], item.Count)));
		}
		public static JsonSoldiers From(Soldiers soldiers)
		{
			return new JsonSoldiers { Items = soldiers.Select(p => new Pair { Type = p.Type.Id, Count = p.Count }) };
		}
	}
}
