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
        public Settings(double interest, double defaultInstability, uint defaultMoney, uint debtLimit, ImmutableArray<string> robotNames, int maxRobotCount, bool singleClient, bool started)
        {
            Interest = interest;
            DefaultInstability = defaultInstability;
            DefaultMoney = defaultMoney;
            DebtLimit = debtLimit;
            RobotNames = robotNames;
            MaxRobotCount = maxRobotCount;
            SingleClient = singleClient;
            Started = started;
        }
        public Settings Start() => new Settings(Interest, DefaultInstability, DefaultMoney, DebtLimit, RobotNames, MaxRobotCount, SingleClient, true);
        static double MinLoanRepayment(uint loan, uint time, double mul) => loan * Math.Pow(mul, time) * (mul - 1) / (Math.Pow(mul, time) - 1);
        public uint LoanRepayment(uint loan, uint time) => (uint)Math.Ceiling(MinLoanRepayment(loan, time, 1.0 + Interest));
        public uint LoanDebt(uint loan, uint time) => (uint)Math.Ceiling(time * MinLoanRepayment(loan, time, 1.0 + Interest));
        public double Instability(uint soldiers, uint defaultSoldiers) => DefaultInstability * Math.Max((int)defaultSoldiers - soldiers, 0) / defaultSoldiers;
        public string RobotName(int i) => i < RobotNames.Length ? RobotNames[i] : "AI " + i;
    }
}