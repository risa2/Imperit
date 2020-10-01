using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Imperit.Pages
{
	public class NewGameModel
	{
		[Range(0.0, double.MaxValue, ErrorMessage = "Záporný úrok je okrádání vìøitele")]
		public double Interest { get; set; }
		[Range(0.0, 1.0, ErrorMessage = "Pravdìpodobnost je èíslo od 0 do 1")]
		public double DefaultInstability { get; set; }
		[Range(0, int.MaxValue, ErrorMessage = "Èlovìk se nemùže narodit zadlužený")]
		public int DefaultMoney { get; set; }
		[Range(0, int.MaxValue, ErrorMessage = "Záporný dluhový limit není splnitelný")]
		public int DebtLimit { get; set; }
		public bool SingleClient { get; set; }
		public string OldPassword { get; set; } = "";
		public string NewPassword { get; set; } = "";
		const string words = @"(\p{Z}*[\p{L}\p{N}]+\p{Z}*)+";
		[RegularExpression(words + "(," + words + ")*", ErrorMessage = "Napiš prosím pouze jména robotù oddìlená èárkou")]
		public string RobotNames { get; set; } = "";
		[Range(0, int.MaxValue, ErrorMessage = "Záporný poèet robotù není možný")]
		public int MaxRobotCount { get; set; }
		public void Init(State.Settings old)
		{
			Interest = old.Interest;
			DefaultInstability = old.DefaultInstability;
			DefaultMoney = old.DefaultMoney;
			DebtLimit = old.DebtLimit;
			SingleClient = old.SingleClient;
			RobotNames = string.Join(", ", old.RobotNames);
			MaxRobotCount = old.MaxRobotCount;
		}
		public State.Settings GetSettings(State.Settings old)
		{
			return new State.Settings(false, SingleClient, Interest, DefaultInstability, DefaultMoney, DebtLimit, RobotNames.Split(',').Select(name => name.Trim()).ToImmutableArray(), MaxRobotCount, old.SeaColor, old.LandColor, old.MountainsColor, old.MountainsWidth, old.SoldierTypes, old.DefaultSoldierTypes, new State.Password(NewPassword));
		}
	}
}
