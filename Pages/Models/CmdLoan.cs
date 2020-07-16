using System.ComponentModel.DataAnnotations;

namespace Imperit.Pages.Models
{
    public class CmdLoan
    {
        readonly State.Settings settings;
        public CmdLoan(State.Settings set) => settings = set;
        [Range(1, int.MaxValue, ErrorMessage = "Tuto částku si nemůžeš půjčit")]
        public int Amount { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Tak dlouho nemůžeš splácet")]
        public int Time { get; set; } = 5;
        public uint Debt => settings.LoanDebt((uint)Amount, (uint)Time);
        public uint Repayment => settings.LoanRepayment((uint)Amount, (uint)Time);
    }
}
