namespace Imperit.State
{
	public class Army
	{
		public Soldiers Soldiers { get; }
		private readonly Player player;
		public int PlayerId => player.Id;
		public Color Color => player.Color;
		public bool IsControlledBySavage => player is Savage;
		public Army(Soldiers soldiers, Player plr)
		{
			Soldiers = soldiers;
			player = plr;
		}
		public Army Join(Army another) => new Army(Soldiers.Add(another.Soldiers), player);
		public Army Subtract(Army another) => new Army(Soldiers.Subtract(another.Soldiers), player);
		public bool IsControlledBy(int player) => PlayerId == player;
		public bool IsAllyOf(Army another) => another.IsControlledBy(PlayerId);
		public int AttackPower => Soldiers.AttackPower;
		public int DefensePower => Soldiers.DefensePower;
		public int Price => Soldiers.Price;
		public bool AnySoldiers => Soldiers.Any;
		public Army AttackedBy(Army another)
		{
			var soldiers = Soldiers.AttackedBy(another.Soldiers);
			return DefensePower >= another.AttackPower ? new Army(soldiers, player) : new Army(soldiers, another.player);
		}
		public bool CanMove(IProvinces provinces, int from, int to) => Soldiers.CanMove(provinces, from, to);
	}
}