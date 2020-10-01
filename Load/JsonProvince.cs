using Imperit.State;
using System.Collections.Generic;

namespace Imperit.Load
{
	public class JsonProvince : IConvertibleToWith<Province, (Settings, IReadOnlyList<Player>, IReadOnlyList<Shape>)>
	{
		public string Type { get; set; } = "";
		public string Name { get; set; } = "";
		public JsonArmy Army { get; set; } = new JsonArmy();
		public JsonArmy DefaultArmy { get; set; } = new JsonArmy();
		public int? Earnings { get; set; }
		public bool? IsStart { get; set; }
		public Province Convert(int i, (Settings, IReadOnlyList<Player>, IReadOnlyList<Shape>) arg)
		{
			var (settings, players, shapes) = arg;
			return Type switch
			{
				"S" => new Sea(i, Name, shapes[i], Army.Convert(i, players), DefaultArmy.Convert(i, players), settings),
				"L" => new Land(i, Name, shapes[i], Army.Convert(i, players), DefaultArmy.Convert(i, players), IsStart ?? false, Earnings ?? 0, settings),
				"P" => new Port(i, Name, shapes[i], Army.Convert(i, players), DefaultArmy.Convert(i, players), IsStart ?? false, Earnings ?? 0, settings),
				"M" => new Mountains(i, Name, shapes[i], Army.Convert(i, players), DefaultArmy.Convert(i, players), settings),
				_ => throw new System.Exception("Unknown State.Province type: " + Type)
			};
		}
		public static JsonProvince From(Province prov)
		{
			return prov switch
			{
				Port Port => new JsonProvince() { Type = "P", Name = Port.Name, Army = JsonArmy.From(Port.Army), DefaultArmy = JsonArmy.From(Port.DefaultArmy), IsStart = Port.IsStart, Earnings = Port.Earnings },
				Land Land => new JsonProvince() { Type = "L", Name = Land.Name, Army = JsonArmy.From(Land.Army), DefaultArmy = JsonArmy.From(Land.DefaultArmy), IsStart = Land.IsStart, Earnings = Land.Earnings },
				Sea Sea => new JsonProvince() { Type = "S", Name = Sea.Name, Army = JsonArmy.From(Sea.Army), DefaultArmy = JsonArmy.From(Sea.DefaultArmy) },
				Mountains Mountains => new JsonProvince() { Type = "M", Name = Mountains.Name, Army = JsonArmy.From(Mountains.Army), DefaultArmy = JsonArmy.From(Mountains.DefaultArmy) },
				_ => throw new System.Exception("Unknown State.Province type: " + prov.GetType())
			};
		}
	}
}