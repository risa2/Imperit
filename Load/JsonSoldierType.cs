using System.Collections.Immutable;
using System.Collections.Generic;
using Imperit.State;
using Imperit.State.SoldierTypes;

namespace Imperit.Load
{
	public class JsonSoldierType : IConvertibleToWith<SoldierType, bool>
	{
		public string Type { get; set; } = "";
		public JsonDescription Description { get; set; } = new JsonDescription();
		public int AttackPower { get; set; }
		public int DefensePower { get; set; }
		public int Weight { get; set; }
		public int Price { get; set; }
		public int? Capacity { get; set; }
		public int? Speed { get; set; }
		public IEnumerable<int>? RecruitPlaces { get; set; }
		public SoldierType Convert(int i, bool _b) => Type switch
		{
			"P" => new Pedestrian(i, Description.Convert(), AttackPower, DefensePower, Weight, Price),
			"S" => new Ship(i, Description.Convert(), AttackPower, DefensePower, Weight, Price, Capacity.Must()),
			"E" => new Elephant(i, Description.Convert(), AttackPower, DefensePower, Weight, Price, Capacity.Must(), Speed.Must(), RecruitPlaces!.ToImmutableArray()),
			"ES" => new ElephantShip(i, Description.Convert(), AttackPower, DefensePower, Weight, Price, Capacity.Must(), Speed.Must(), RecruitPlaces!.ToImmutableArray()),
			_ => throw new System.Exception("Unknown State.SoldierType type: " + Type)
		};
		public static JsonSoldierType From(SoldierType type) => type switch
		{
			Pedestrian P => new JsonSoldierType { Type = "P", Description = JsonDescription.From(P.Description), AttackPower = P.AttackPower, DefensePower = P.DefensePower, Weight = P.Weight, Price = P.Price },
			Ship S => new JsonSoldierType { Type = "S", Description = JsonDescription.From(S.Description), AttackPower = S.AttackPower, DefensePower = S.DefensePower, Weight = S.Weight, Price = S.Price, Capacity = S.Capacity },
			Elephant E => new JsonSoldierType { Type = "E", Description = JsonDescription.From(E.Description), AttackPower = E.AttackPower, DefensePower = E.DefensePower, Weight = E.Weight, Price = E.Price, Capacity = E.Capacity, Speed = E.Speed, RecruitPlaces = E.RecruitPlaces },
			ElephantShip ES => new JsonSoldierType { Type = "ES", Description = JsonDescription.From(ES.Description), AttackPower = ES.AttackPower, DefensePower = ES.DefensePower, Weight = ES.Weight, Price = ES.Price, Capacity = ES.Capacity, Speed = ES.Speed, RecruitPlaces = ES.RecruitPlaces },
			_ => throw new System.Exception("Unknown State.SoldierType type: " + type.GetType())
		};
	}
}
