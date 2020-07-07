namespace Imperit.State
{
    public readonly struct PlayerPower
    {
        public readonly uint Soldiers, Money, Income;
        public PlayerPower(uint soldiers, uint money, uint income)
        {
            Soldiers = soldiers;
            Money = money;
            Income = income;
        }
    }
}