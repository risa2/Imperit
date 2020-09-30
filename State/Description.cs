using System;

namespace Imperit.State
{
    public class Description : IEquatable<Description>
    {
        public readonly string Name, Symbol, Text;
        public Description(string name, string symbol, string text)
        {
            Name = name;
            Symbol = symbol;
            Text = text;
        }
        public bool Equals(Description? d2) => (Name, Symbol, Text) == (d2?.Name, d2?.Symbol, d2?.Text);
        public override bool Equals(object? obj) => obj is Description d2 && Equals(d2);
        public override int GetHashCode() => (Name, Symbol, Text).GetHashCode();
    }
}
