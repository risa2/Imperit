using System.ComponentModel.DataAnnotations;

namespace Imperit.Pages.Models
{
    public enum MoveType { Attack, Reinforcement }
    public class CmdMove
    {
        [Range(1, int.MaxValue, ErrorMessage = "Tento počet vojáků nelze naverbovat")]
        public int Soldiers { get; set; }
        public MoveType Type { get; set; }
        public bool IsAttack => Type == MoveType.Attack;
    }
}
