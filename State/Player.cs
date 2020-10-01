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
		public int Income { get; }
		public bool Alive { get; }
		public ImmutableArray<SoldierType> SoldierTypes { get; }
		public Player(int id, string name, Color color, Password password, int money, bool alive, int income, ImmutableArray<SoldierType> soldierTypes)
		{
			Id = id;
			Name = name;
			Color = color;
			Password = password;
			Money = money;
			Alive = alive;
			Income = income;
			SoldierTypes = soldierTypes;
		}
		public virtual Player GainMoney(int amount) => new Player(Id, Name, Color, Password, Money + amount, Alive, Income, SoldierTypes);
		public virtual Player Pay(int amount) => new Player(Id, Name, Color, Password, Money - amount, Alive, Income, SoldierTypes);
		public virtual Player Die() => new Player(Id, Name, Color, Password, 0, false, 0, ImmutableArray<SoldierType>.Empty);
		public virtual Player ChangeIncome(int change) => new Player(Id, Name, Color, Password, Money, Alive, (Income + change), SoldierTypes);
		public virtual Player AddSoldierTypes(params SoldierType[] types) => new Player(Id, Name, Color, Password, Money, Alive, Income, SoldierTypes.AddRange(types));
		public Player Earn() => GainMoney(Income);
		public override bool Equals(object? obj) => obj is Player p && p.Id == Id;
		public override int GetHashCode() => Id.GetHashCode();
		public static bool operator ==(Player? a, Player? b) => (a is null && b is null) || (a is Player x && x.Equals(b));
		public static bool operator !=(Player? a, Player? b) => ((a is null) != (b is null)) || (a is Player x && !x.Equals(b));
	}
}