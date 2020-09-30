using Imperit.State;

namespace Imperit.Pages
{
	public class MoveModel
	{
		public enum Type { Attack, Reinforcement }
		public IntModel[] Soldiers = System.Array.Empty<IntModel>();
		public SoldierType[] SoldierTypes = System.Array.Empty<SoldierType>();
		public Type MoveType { get; set; }
		public bool IsAttack
		{
			get => MoveType == Type.Attack;
			set => MoveType = value ? Type.Attack : Type.Reinforcement;
		}
	}
}
