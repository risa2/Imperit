using System.Collections.Generic;
using System.Collections.Immutable;

namespace Imperit.Load
{
    public class Settings : IConvertibleToWith<State.Settings, bool>
    {
        public uint DefaultMoney { get; set; }
        public uint DebtLimit { get; set; }
        public double Interest { get; set; }
        public double DefaultInstability { get; set; }
        public bool SingleClient { get; set; }
        public bool Started { get; set; }
        public IEnumerable<string>? RobotNames { get; set; }
        public int MaxRobotCount { get; set; }
        public string? SeaColor { get; set; }
        public string? LandColor { get; set; }
        public string? MountainsColor { get; set; }
        public int MountainsWidth { get; set; }
        public State.Settings Convert(int useless_i, bool useless_b) => new State.Settings(Started, SingleClient, Interest, DefaultInstability, DefaultMoney, DebtLimit, RobotNames!.ToImmutableArray(), MaxRobotCount, State.Color.Parse(SeaColor!), State.Color.Parse(LandColor!), State.Color.Parse(MountainsColor!), MountainsWidth);
        public static Settings FromSettings(State.Settings s) => new Settings() { Interest = s.Interest, DefaultInstability = s.DefaultInstability, DefaultMoney = s.DefaultMoney, DebtLimit = s.DebtLimit, SingleClient = s.SingleClient, Started = s.Started, RobotNames = s.RobotNames, MaxRobotCount = s.MaxRobotCount, LandColor = s.LandColor.ToString(), SeaColor = s.SeaColor.ToString(), MountainsColor = s.MountainsColor.ToString(), MountainsWidth = s.MountainsWidth };
    }
}