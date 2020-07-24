namespace Imperit.State
{
    public readonly struct PlayerPower
    {
        public readonly long Total;
        public readonly double Change, Ratio;
        public PlayerPower(long total, double change, double ratio)
        {
            Total = total;
            Change = change;
            Ratio = ratio;
        }
    }
}