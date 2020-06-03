using System.ComponentModel.DataAnnotations;

namespace Imperit.Pages.Models
{
    public class NewGame
    {
        [Range(0.0, 1.0)] public double Interest { get; set; }
        [Range(0.0, 1.0)] public double DefaultInstability { get; set; }
        public int DefaultMoney { get; set; }
        public int DebtLimit { get; set; }
        public bool SingleClient { get; set; }
        public string OldPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }
}
