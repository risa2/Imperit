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
        public State.Player Convert(int i, bool useless) => IsHuman ? new State.Player(i, Name, new State.Color(r, g, b), new State.Password(System.Convert.FromBase64String(Password)), Money, Credibility, Alive, Income)
                                                                    : new State.Robot(i, Name, new State.Color(r, g, b), new State.Password(System.Convert.FromBase64String(Password)), Money, Credibility, Alive, Income);
        public static Player FromPlayer(State.Player p) => new Player() { Name = p.Name, r = p.Color.r, g = p.Color.g, b = p.Color.b, IsHuman = !(p is State.Robot), Password = System.Convert.ToBase64String(p.Password.Hash), Money = p.Money, Alive = p.Alive, Credibility = p.Credibility, Income = p.Income };
    }
}