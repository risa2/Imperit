namespace Imperit.State
{
	public readonly struct PlayerPower : System.IEquatable<PlayerPower>
	{
		public readonly long Total;
		public readonly double Change, Ratio;
		public PlayerPower(long total, double change, double ratio)
		{
			Total = total;
			Change = change;
			Ratio = ratio;
		}
		public override bool Equals(object? obj) => obj is PlayerPower pp && pp.Equals(this);
		public override int GetHashCode() => (Total, Change, Ratio).GetHashCode();
		public static bool operator ==(PlayerPower left, PlayerPower right) => left.Equals(right);
		public static bool operator !=(PlayerPower left, PlayerPower right) => !left.Equals(right);
		public bool Equals(PlayerPower other) => (Total, Change, Ratio) == (other.Total, other.Change, other.Ratio);
	}
}