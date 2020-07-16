namespace Imperit.Load
{
    public class Player : IConvertibleToWith<State.Player, bool>
    {
        public string Name { get; set; } = "";
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public bool IsHuman { get; set; }
        public string Password { get; set; } = "";
        public uint Money { get; set; }
        public bool Alive { get; set; }
        public uint Income { get; set; }
        public State.Player Convert(int i, bool useless) => IsHuman ? new State.Player(i, Name, new State.Color(R, G, B), new State.Password(System.Convert.FromBase64String(Password)), Money, Alive, Income)
                                                                    : new State.Robot(i, Name, new State.Color(R, G, B), new State.Password(System.Convert.FromBase64String(Password)), Money, Alive, Income);
        public static Player FromPlayer(State.Player p) => new Player() { Name = p.Name, R = p.Color.r, G = p.Color.g, B = p.Color.b, IsHuman = !(p is State.Robot), Password = System.Convert.ToBase64String(p.Password.Hash), Money = p.Money, Alive = p.Alive, Income = p.Income };
    }
}