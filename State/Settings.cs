using static System.Math;

namespace Imperit.State
{
    public class Settings
    {
        public readonly double Interest, DefaultInstability;
        public readonly uint DefaultMoney, DebtLimit;
        public readonly bool SingleClient, Started;
        public readonly string[] RobotNames;
        public readonly int MaxRobotCount;
        public Settings(double interest, double defaultInstability, uint defaultMoney, uint debtLimit, bool singleClient, bool started, string[] robotNames, int maxRobotCount)
        {
            Interest = interest;
            DefaultInstability = defaultInstability;
            DefaultMoney = defaultMoney;
            DebtLimit = debtLimit;
            SingleClient = singleClient;
            Started = started;
            RobotNames = robotNames;
            MaxRobotCount = maxRobotCount;
        }
        public Settings Start() => new Settings(Interest, DefaultInstability, DefaultMoney, DebtLimit, SingleClient, true, RobotNames, MaxRobotCount);
        static double MinLoanRepayment(uint loan, uint time, double mul) => loan * mul.Pow(time) * (mul - 1) / mul / (mul.Pow(time) - 1);
        public uint LoanRepayment(uint loan, uint time, double credibility) => (uint)Ceiling(MinLoanRepayment(loan, time + 1, 1.0 + Interest / credibility));
        public uint LoanDebt(uint loan, uint time, double credibility) => (uint)Ceiling(time * MinLoanRepayment(loan, time + 1, 1.0 + Interest / credibility));
        public double Instability(uint soldiers, double credibility) => DefaultInstability / Pow(2, soldiers / 50.0) / credibility;
        public string RobotName(int i) => i < RobotNames.Length ? RobotNames[i] : "AI " + i;
    }
}