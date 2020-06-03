namespace Imperit.State
{
    public readonly struct PlayerPower
    {
        public readonly uint Soldiers;
        public readonly uint Money;
        public readonly uint Debt;
        public readonly uint Income;
        public PlayerPower(uint soldiers, uint money, uint debt, uint income)
        {
            Soldiers = soldiers;
            Money = money;
            Debt = debt;
            Income = income;
        }
    }
}