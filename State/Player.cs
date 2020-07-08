using System;

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
        public readonly double Credibility;
        public Player(int id, string name, Color color, Password password, uint money, double credibility, bool alive, uint income)
        {
            Id = id;
            Name = name;
            Color = color;
            Password = password;
            Money = money;
            Credibility = credibility;
            Alive = alive;
            Income = income;
        }
        protected double CredibilityChanged(double change) => Math.Max(0.0001, Credibility * Math.Pow(2, change));
        public virtual Player LoseCredibility(double amount) => new Player(Id, Name, Color, Password, Money, CredibilityChanged(-amount), Alive, Income);
        public virtual Player GainMoney(uint amount) => new Player(Id, Name, Color, Password, Money + amount, Credibility, Alive, Income);
        public virtual Player Pay(uint amount) => new Player(Id, Name, Color, Password, Money - amount, Credibility, Alive, Income);
        public Player Earn() => GainMoney(Income);
        public virtual Player Die() => new Player(Id, Name, Color, Password, 0, 1.0, false, 0);
        public virtual Player IncreaseIncome(uint change) => new Player(Id, Name, Color, Password, Money, Credibility, Alive, Income + change);
        public virtual Player DecreaseIncome(uint change) => new Player(Id, Name, Color, Password, Money, Credibility, Alive, Income - change);
        public bool Surviving => Income > 0;
        public override bool Equals(object? obj) => obj != null && obj is Player p && p.Id == Id;
        public override int GetHashCode() => Id.GetHashCode();
        public static bool operator ==(Player? a, Player? b) => (a is null && b is null) || (a is object x && x.Equals(b));
        public static bool operator !=(Player? a, Player? b) => ((a is null) != (b is null)) || (a is object x && !x.Equals(b));
    }
}