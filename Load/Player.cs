namespace Imperit.Load
{
    public class Player : IConvertibleToWith<State.Player, bool>
    {
        public string Name { get; set; } = "";
        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }
        public bool IsHuman { get; set; }
        public string Password { get; set; } = "";
        public uint Money { get; set; }
        public bool Alive { get; set; }
        public double Credibility { get; set; }
        public uint Income { get; set; }
        public State.Player Convert(int i, bool useless) => new State.Player(id: i, name: Name, color: new State.Color(r, g, b), isHuman: IsHuman, password: new State.Password(System.Convert.FromBase64String(Password)), money: Money, credibility: Credibility, alive: Alive, income: Income);
        public static Player FromPlayer(State.Player p) => new Player() { Name = p.Name, r = p.Color.r, g = p.Color.g, b = p.Color.b, IsHuman = p.IsHuman, Password = System.Convert.ToBase64String(p.Password.Hash), Money = p.Money, Alive = p.Alive, Credibility = p.Credibility, Income = p.Income };
    }
}