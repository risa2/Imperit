using System;
using System.Collections.Immutable;

namespace Imperit.State
{
    public class Settings
    {
        public readonly double Interest, DefaultInstability;
        public readonly uint DefaultMoney, DebtLimit;
        public readonly ImmutableArray<string> RobotNames;
        public readonly int MaxRobotCount;
        public readonly bool SingleClient, Started;
        public readonly Color SeaColor, LandColor, MountainsColor;
        public Settings(double interest, double defaultInstability, uint defaultMoney, uint debtLimit, ImmutableArray<string> robotNames, int maxRobotCount, bool singleClient, Color seaColor, Color landColor, Color mountainsColor, bool started)
        {
            Interest = interest;
            DefaultInstability = defaultInstability;
            DefaultMoney = defaultMoney;
            DebtLimit = debtLimit;
            RobotNames = robotNames;
            MaxRobotCount = maxRobotCount;
            SingleClient = singleClient;
            Started = started;
            SeaColor = seaColor;
            LandColor = landColor;
            MountainsColor = mountainsColor;
        }
        public Settings Start() => new Settings(Interest, DefaultInstability, DefaultMoney, DebtLimit, RobotNames, MaxRobotCount, SingleClient, SeaColor, LandColor, MountainsColor, true);
        public double Instability(uint soldiers, uint defaultSoldiers) => DefaultInstability * Math.Max((int)defaultSoldiers - soldiers, 0) / defaultSoldiers;
        public string RobotName(int i) => i < RobotNames.Length ? RobotNames[i] : "AI " + i;
    }
}