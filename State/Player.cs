using System.Collections.Immutable;

namespace Imperit.State
{
	public class Player
	{
		public int Id { get; }
		public string Name { get; }
		public Color Color { get; }
		public Password Password { get; }
		public int Money { get; }
		public bool Alive { get; }
		public ImmutableArray<SoldierType> SoldierTypes { get; }
		public Player(int id, string name, Color color, Password password, int money, bool alive, ImmutableArray<SoldierType> soldierTypes)
		{
			Id = id;
			Name = name;
			Color = color;
			Password = password;
			Money = money;
			Alive = alive;
			SoldierTypes = soldierTypes;
		}
		public virtual Player ChangeMoney(int amount) => new Player(Id, Name, Color, Password, Money + amount, Alive, SoldierTypes);
		public virtual Player ChangeIncome(int change) => new Player(Id, Name, Color, Password, Money, Alive, SoldierTypes);
		public virtual Player Die() => new Player(Id, Name, Color, Password, 0, false, ImmutableArray<SoldierType>.Empty);
		public virtual Player AddSoldierTypes(params SoldierType[] types) => new Player(Id, Name, Color, Password, Money, Alive, SoldierTypes.AddRange(types));
		public override bool Equals(object? obj) => obj is Player p && p.Id == Id;
		public override int GetHashCode() => Id.GetHashCode();
		public static bool operator ==(Player? a, Player? b) => (a is null && b is null) || (a is Player x && x.Equals(b));
		public static bool operator !=(Player? a, Player? b) => ((a is null) != (b is null)) || (a is Player x && !x.Equals(b));
	}
}