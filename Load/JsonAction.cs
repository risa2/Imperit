using Imperit.Dynamics.Actions;
using Imperit.State;
using System.Collections.Generic;

namespace Imperit.Load
{
	public class JsonAction : IConvertibleToWith<Dynamics.IAction, (Settings, IReadOnlyList<Player>)>
	{
		public string? Type { get; set; }
		public int? Province { get; set; }
		public JsonArmy? Army { get; set; }
		public int? Player { get; set; }
		public int? Amount { get; set; }
		public int? Debt { get; set; }
		public Dynamics.IAction Convert(int i, (Settings, IReadOnlyList<Player>) arg)
		{
			var (settings, players) = arg;
			return Type switch
			{
				"Reinforcement" => new Reinforcement(Province.Must(), Army!.Convert(i, players)),
				"Battle" => new Battle(Province.Must(), Army!.Convert(i, players)),
				"Earnings" => new Earnings(),
				"Instability" => new Instability(),
				"Loan" => new Loan(Player.Must(), Debt ?? 0, settings),
				"Mortality" => new Mortality(),
				_ => throw new System.Exception("Unknown type of Action: " + Type)
			};

		}
		public static JsonAction From(Dynamics.IAction action)
		{
			return action switch
			{
				Reinforcement Add => new JsonAction() { Type = "Reinforcement", Province = Add.Province, Army = JsonArmy.From(Add.Army) },
				Battle Attack => new JsonAction() { Type = "Battle", Province = Attack.Province, Army = JsonArmy.From(Attack.Army) },
				Earnings _ => new JsonAction() { Type = "Earnings" },
				Instability _ => new JsonAction() { Type = "Instability" },
				Loan Loan => new JsonAction() { Type = "Loan", Player = Loan.Debtor, Debt = Loan.Debt },
				Mortality _ => new JsonAction() { Type = "Mortality" },
				_ => throw new System.Exception("Unknown type of Action: " + action.GetType())
			};
		}
	}
}