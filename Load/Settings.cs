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
        public string[]? RobotNames { get; set; }
        public int MaxRobotCount { get; set; }
        public State.Settings Convert(int useless_i, bool useless_b) => new State.Settings(Interest, DefaultInstability, DefaultMoney, DebtLimit, RobotNames ?? System.Array.Empty<string>(), MaxRobotCount, SingleClient, Started);
        public static Settings FromSettings(State.Settings s) => new Settings() { Interest = s.Interest, DefaultInstability = s.DefaultInstability, DefaultMoney = s.DefaultMoney, DebtLimit = s.DebtLimit, SingleClient = s.SingleClient, Started = s.Started, RobotNames = s.RobotNames, MaxRobotCount = s.MaxRobotCount };
    }
}