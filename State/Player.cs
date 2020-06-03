using System;

namespace Imperit.State
{
    public class Player
    {
        public int Id { get; }
        public readonly string Name;
        public readonly Color Color;
        public readonly Password Password;
        public readonly uint Money, Income;
        public readonly bool Alive, IsHuman;
        public readonly double Credibility;
        public Player(int id, string name, Color color, bool isHuman, Password password, uint money, double credibility, bool alive, uint income)
        {
            Id = id;
            Name = name;
            Color = color;
            IsHuman = isHuman;
            Password = password;
            Money = money;
            Credibility = credibility;
            Alive = alive;
            Income = income;
        }
        public Player LoseCredibility(double amount) => new Player(Id, Name, Color, IsHuman, Password, Money, Math.Max(0.01, Credibility / Math.Pow(2, amount)), Alive, Income);
        public Player GainMoney(uint amount) => new Player(Id, Name, Color, IsHuman, Password, Money + amount, Credibility, Alive, Income);
        public Player Pay(uint amount) => new Player(Id, Name, Color, IsHuman, Password, Money - amount, Credibility, Alive, Income);
        public Player Earn() => GainMoney(Income);
        public Player Die() => new Player(Id, Name, Color, IsHuman, Password, 0, 1.0, false, 0);
        public Player IncreaseIncome(uint change) => new Player(Id, Name, Color, IsHuman, Password, Money, Credibility, Alive, Income + change);
        public Player DecreaseIncome(uint change) => new Player(Id, Name, Color, IsHuman, Password, Money, Credibility, Alive, Income - change);
        public bool Surviving => Income > 0;

        public override bool Equals(object? obj) => obj != null && obj is Player p && p.Id == Id;
        public override int GetHashCode() => Id.GetHashCode();
        public static bool operator ==(Player? a, Player? b) => ((object?)a == null && (object?)b == null) || ((object?)a != null && a.Equals(b));
        public static bool operator !=(Player? a, Player? b) => (((object?)a == null) != ((object?)b == null)) || ((object?)a != null && !a.Equals(b));
    }
}