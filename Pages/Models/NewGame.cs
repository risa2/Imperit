using System.ComponentModel.DataAnnotations;

namespace Imperit.Pages.Models
{
    public class NewGame
    {
        [Range(0.0, double.MaxValue, ErrorMessage = "Záporný úrok je okrádání vìøitele")]
        public double Interest { get; set; }
        [Range(0.0, 1.0, ErrorMessage = "Pravdìpodobnost je èíslo od 0 do 1")]
        public double DefaultInstability { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Èlovìk se nemùže narodit zadlužený")]
        public int DefaultMoney { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Záporný dluhový limit není splnitelný")]
        public int DebtLimit { get; set; }
        public bool SingleClient { get; set; }
        public string OldPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
        const string words = @"(\p{Z}*[\p{L}\p{N}]+\p{Z}*)+";
        [RegularExpression(words + "(," + words + ")*", ErrorMessage = "Napiš prosím pouze jména robotù oddìlená èárkou")]
        public string RobotNames { get; set; } = "";
        [Range(0, int.MaxValue, ErrorMessage = "Záporný poèet robotù není možný")]
        public int MaxRobotCount { get; set; }
    }
}
