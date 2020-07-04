using System;
using System.Transactions;

namespace Imperit.State
{
    public class Settings
    {
        public readonly uint DefaultMoney, DebtLimit;
        public readonly double Interest, DefaultInstability;
        public readonly bool SingleClient, Started;
        public Settings(double interest, double defaultInstability, uint defaultMoney, uint debtLimit, bool singleClient, bool started)
        {
            Interest = interest;
            DefaultInstability = defaultInstability;
            DefaultMoney = defaultMoney;
            DebtLimit = debtLimit;
            SingleClient = singleClient;
            Started = started;
        }
        public Settings Start() => new Settings(Interest, DefaultInstability, DefaultMoney, DebtLimit, SingleClient, true);
        static double MinLoanRepayment(uint loan, uint time, double mul) => loan * mul.Pow(time) * (mul - 1) / mul / (mul.Pow(time) - 1);
        public uint LoanRepayment(uint loan, uint time, double credibility) => (uint)Math.Ceiling(MinLoanRepayment(loan, time, 1.0 + Interest / credibility));
        public uint LoanDebt(uint loan, uint time, double credibility) => (uint)Math.Ceiling(time * MinLoanRepayment(loan, time, 1.0 + Interest / credibility));
        public double Instability(uint soldiers, double credibility) => DefaultInstability / Math.Pow(2, soldiers / 50.0) / credibility;
    }
}