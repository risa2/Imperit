namespace Imperit.State
{
	public class Savage : Player
	{
		public Savage(int id)
			: base(id, "", new Color(), new Password(""), 0, true) { }
		public override Player ChangeMoney(int amount) => new Savage(Id);
		public override Player Die() => new Savage(Id);
		public override Player ChangeIncome(int change) => new Savage(Id);
	}
}
