using System;
using System.Collections.Immutable;

namespace Imperit.State
{
	public class Settings
	{
		public readonly bool SingleClient, Started;
		public readonly double Interest, DefaultInstability;
		public readonly int DefaultMoney, DebtLimit;
		public readonly ImmutableArray<string> RobotNames;
		public readonly int MaxRobotCount, MountainsWidth;
		public readonly Color SeaColor, LandColor, MountainsColor;
		public readonly ImmutableArray<SoldierType> SoldierTypes;
		public readonly Password Password;
		public Settings(bool started, bool singleClient, double interest, double defaultInstability, int defaultMoney, int debtLimit, ImmutableArray<string> robotNames, int maxRobotCount, Color seaColor, Color landColor, Color mountainsColor, int mountainsWidth, ImmutableArray<SoldierType> soldierTypes, Password password)
		{
			Started = started;
			SingleClient = singleClient;
			Interest = interest;
			DefaultInstability = defaultInstability;
			DefaultMoney = defaultMoney;
			DebtLimit = debtLimit;
			RobotNames = robotNames;
			MaxRobotCount = maxRobotCount;
			SeaColor = seaColor;
			LandColor = landColor;
			MountainsColor = mountainsColor;
			MountainsWidth = mountainsWidth;
			SoldierTypes = soldierTypes;
			Password = password;
		}
		public Settings Start() => new Settings(true, SingleClient, Interest, DefaultInstability, DefaultMoney, DebtLimit, RobotNames, MaxRobotCount, SeaColor, LandColor, MountainsColor, MountainsWidth, SoldierTypes, Password);
		public double Instability(Soldiers now, Soldiers start) => DefaultInstability * Math.Max(start.DefensePower - now.DefensePower - 1, -1) / start.DefensePower;
		public string RobotName(int i) => i < RobotNames.Length ? RobotNames[i] : "AI " + i;
	}
}