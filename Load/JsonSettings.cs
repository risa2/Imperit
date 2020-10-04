using Imperit.State;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Imperit.Load
{
	public class JsonSettings : IConvertibleToWith<Settings, bool>
	{
		public int DebtLimit { get; set; }
		public double DefaultInstability { get; set; }
		public int DefaultMoney { get; set; }
		public double Interest { get; set; }
		public string LandColor { get; set; } = "";
		public int MaxRobotCount { get; set; }
		public string MountainsColor { get; set; } = "";
		public int MountainsWidth { get; set; }
		public string Password { get; set; } = "";
		public ImmutableArray<string> RobotNames { get; set; }
		public string SeaColor { get; set; } = "";
		public bool SingleClient { get; set; }
		public IEnumerable<JsonSoldierType>? SoldierTypes { get; set; }
		public bool Started { get; set; }
		public Settings Convert(int _i, bool _b) => new Settings(Started, SingleClient, Interest, DefaultInstability, DefaultMoney, DebtLimit, RobotNames, MaxRobotCount, Color.Parse(SeaColor), Color.Parse(LandColor), Color.Parse(MountainsColor), MountainsWidth, SoldierTypes.Select((t, i) => t.Convert(i, false)).ToImmutableArray(), State.Password.Parse(Password));
		public static JsonSettings From(Settings s) => new JsonSettings() { Started = s.Started, SingleClient = s.SingleClient, Interest = s.Interest, DefaultInstability = s.DefaultInstability, DefaultMoney = s.DefaultMoney, DebtLimit = s.DebtLimit, RobotNames = s.RobotNames, MaxRobotCount = s.MaxRobotCount, LandColor = s.LandColor.ToString(), SeaColor = s.SeaColor.ToString(), MountainsColor = s.MountainsColor.ToString(), MountainsWidth = s.MountainsWidth, SoldierTypes = s.SoldierTypes.Select(t => JsonSoldierType.From(t)), Password = s.Password.ToString() };
	}
}