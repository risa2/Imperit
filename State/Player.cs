namespace Imperit.State
{
    public class Player
    {
        public readonly int Id;
        public readonly string Name;
        public readonly Color Color;
        public readonly Password Password;
        public readonly uint Money, Income;
        public readonly bool Alive;
        public Player(int id, string name, Color color, Password password, uint money, bool alive, uint income)
        {
            Id = id;
            Name = name;
            Color = color;
            Password = password;
            Money = money;
            Alive = alive;
            Income = income;
        }
        public virtual Player GainMoney(uint amount) => new Player(Id, Name, Color, Password, Money + amount, Alive, Income);
        public virtual Player Pay(uint amount) => new Player(Id, Name, Color, Password, Money - amount, Alive, Income);
        public Player Earn() => GainMoney(Income);
        public virtual Player Die() => new Player(Id, Name, Color, Password, 0, false, 0);
        public virtual Player ChangeIncome(int change) => new Player(Id, Name, Color, Password, Money, Alive, (uint)(Income + change));
        public override bool Equals(object? obj) => obj != null && obj is Player p && p.Id == Id;
        public override int GetHashCode() => Id.GetHashCode();
        public static bool operator ==(Player? a, Player? b) => (a is null && b is null) || (a is Player x && x.Equals(b));
        public static bool operator !=(Player? a, Player? b) => ((a is null) != (b is null)) || (a is Player x && !x.Equals(b));
    }
}