using System.Collections.Generic;

namespace Imperit.Load
{
    public class Action : IConvertibleToWith<Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>
    {
        public string? Type { get; set; }
        public int? Province { get; set; }
        public Army? Army { get; set; }
        public int? Player { get; set; }
        public uint? Amount { get; set; }
        public uint? Debt { get; set; }
        public uint? Remaining { get; set; }
        public uint? Repayment { get; set; }
        public Dynamics.IAction Convert(int i, (State.Settings, IReadOnlyList<State.Player>, State.Provinces) arg)
        {
            (var settings, var players, var provinces) = arg;
            return Type switch
            {
                "Attack" => new Dynamics.Actions.Attack(Province.Must(), Army!.Convert(i, (settings, players)), players),
                "Instability" => new Dynamics.Actions.Instability(Province.Must(), Player),
                "Reinforcement" => new Dynamics.Actions.Reinforcement(Province.Must(), Army!.Convert(i, (settings, players)), players),
                "PortRenewal" => new Dynamics.Actions.PortRenewal(Province.Must()),
                "Earn" => new Dynamics.Actions.Earn(Player.Must()),
                "Mortality" => new Dynamics.Actions.Mortality(Player.Must(), provinces),
                "Repayment" => new Dynamics.Actions.Loan(settings, players, Player.Must(), Debt ?? 0, Remaining ?? 0, Repayment ?? 0),
                "IncomeIncrease" => new Dynamics.Actions.IncomeIncrease(Player.Must(), Amount ?? 0),
                "IncomeDecrease" => new Dynamics.Actions.IncomeDecrease(Player.Must(), Amount ?? 0),
                _ => throw new System.Exception("Invalid type of Action: " + Type)
            };

        }
        public static Action FromAction(Dynamics.IAction action)
        {
            return action switch
            {
                Dynamics.Actions.Attack Attack => new Action() { Type = "Attack", Province = Attack.Province, Army = Army.FromArmy(Attack.Army) },
                Dynamics.Actions.Earn Earn => new Action() { Type = "Earn", Player = Earn.Player },
                Dynamics.Actions.Instability Instability => new Action() { Type = "Instability", Player = Instability.LoyalTo, Province = Instability.Land },
                Dynamics.Actions.Mortality Mortality => new Action() { Type = "Mortality", Player = Mortality.Player },
                Dynamics.Actions.PortRenewal PortRenewal => new Action() { Type = "PortRenewal", Province = PortRenewal.Port },
                Dynamics.Actions.Reinforcement Reinforcement => new Action() { Type = "Reinforcement", Province = Reinforcement.Province, Army = Army.FromArmy(Reinforcement.Army) },
                Dynamics.Actions.Loan Loan => new Action() { Type = "Repayment", Player = Loan.Debtor, Debt = Loan.Debt, Remaining = Loan.Remaining, Repayment = Loan.Repayment },
                Dynamics.Actions.IncomeIncrease IncomeIncrease => new Action() { Type = "IncomeIncrease", Player = IncomeIncrease.Player, Amount = IncomeIncrease.Change },
                Dynamics.Actions.IncomeDecrease IncomeDecrease => new Action() { Type = "IncomeDecrease", Player = IncomeDecrease.Player, Amount = IncomeDecrease.Change },
                _ => throw new System.Exception("Invalid type of Action: " + action)
            };
        }
    }
}