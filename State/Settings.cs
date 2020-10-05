using System;
using System.Collections.Immutable;

namespace Imperit.State
{
	public class Settings
	{
		public readonly double DefaultInstability, Interest;
		public readonly int DebtLimit, DefaultMoney;
		public readonly int MountainsWidth;
		public readonly Color LandColor, MountainsColor, SeaColor;
		public readonly Password Password;
		public readonly ImmutableArray<string> RobotNames;
		public readonly ImmutableArray<SoldierType> SoldierTypes;
		public Settings(int debtLimit, double defaultInstability, int defaultMoney, double interest, Color landColor, Color mountainsColor, int mountainsWidth, Password password, ImmutableArray<string> robotNames, Color seaColor, ImmutableArray<SoldierType> soldierTypes)
		{
			DebtLimit = debtLimit;
			DefaultInstability = defaultInstability;
			DefaultMoney = defaultMoney;
			Interest = interest;
			LandColor = landColor;
			MountainsColor = mountainsColor;
			MountainsWidth = mountainsWidth;
			Password = password;
			RobotNames = robotNames;
			SeaColor = seaColor;
			SoldierTypes = soldierTypes;
		}
		public double Instability(Soldiers now, Soldiers start) => DefaultInstability * Math.Max(start.DefensePower - now.DefensePower - 1, -1) / start.DefensePower;
	}
}