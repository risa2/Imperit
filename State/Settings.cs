using System;
using System.Collections.Immutable;

namespace Imperit.State
{
    public class Settings
    {
        public readonly bool SingleClient, Started;
        public readonly double Interest, DefaultInstability;
        public readonly uint DefaultMoney, DebtLimit;
        public readonly ImmutableArray<string> RobotNames;
        public readonly int MaxRobotCount;
        public readonly Color SeaColor, LandColor, MountainsColor;
        public readonly int MountainsWidth;
        public Settings(bool started, bool singleClient, double interest, double defaultInstability, uint defaultMoney, uint debtLimit, ImmutableArray<string> robotNames, int maxRobotCount, Color seaColor, Color landColor, Color mountainsColor, int mountainsWidth)
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
        }
        public Settings Start() => new Settings(true, SingleClient, Interest, DefaultInstability, DefaultMoney, DebtLimit, RobotNames, MaxRobotCount, SeaColor, LandColor, MountainsColor, MountainsWidth);
        public double Instability(uint soldiers, uint defaultSoldiers) => DefaultInstability * Math.Max((int)defaultSoldiers - soldiers, 0) / defaultSoldiers;
        public string RobotName(int i) => i < RobotNames.Length ? RobotNames[i] : "AI " + i;
    }
}