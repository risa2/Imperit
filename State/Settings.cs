using System;

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
        public uint LoanDebt(uint loan, double credibility) => (uint)Math.Ceiling(loan * (1.0 + Interest) / credibility);
        public uint DebtLoan(uint debt, double credibility) => (uint)Math.Floor(debt * credibility / (1.0 + Interest));
        public double Instability(uint soldiers, double credibility) => DefaultInstability / Math.Pow(2, soldiers / 50.0) / credibility;
    }
}