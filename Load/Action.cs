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
            (var settings, var players, _) = arg;
            return Type switch
            {
                "Attack" => new Dynamics.Actions.Attack(Province.Must(), Army!.Convert(i, (settings, players))),
                "Earn" => new Dynamics.Actions.Earn(),
                "IncomeIncrease" => new Dynamics.Actions.IncomeIncrease(Player.Must(), Amount ?? 0),
                "IncomeDecrease" => new Dynamics.Actions.IncomeDecrease(Player.Must(), Amount ?? 0),
                "Instability" => new Dynamics.Actions.Instability(),
                "Loan" => new Dynamics.Actions.Loan(Player.Must(), Debt ?? 0, Remaining ?? 0, Repayment ?? 0, settings),
                "Mortality" => new Dynamics.Actions.Mortality(),
                "PortRenewal" => new Dynamics.Actions.PortRenewal(),
                "Reinforcement" => new Dynamics.Actions.Reinforcement(Province.Must(), Army!.Convert(i, (settings, players))),
                _ => throw new System.Exception("Invalid type of Action: " + Type)
            };

        }
        public static Action FromAction(Dynamics.IAction action)
        {
            return action switch
            {
                Dynamics.Actions.Attack Attack => new Action() { Type = "Attack", Province = Attack.Province, Army = Army.FromArmy(Attack.Army) },
                Dynamics.Actions.Earn _ => new Action() { Type = "Earn" },
                Dynamics.Actions.IncomeIncrease IncomeIncrease => new Action() { Type = "IncomeIncrease", Player = IncomeIncrease.Player, Amount = IncomeIncrease.Change },
                Dynamics.Actions.IncomeDecrease IncomeDecrease => new Action() { Type = "IncomeDecrease", Player = IncomeDecrease.Player, Amount = IncomeDecrease.Change },
                Dynamics.Actions.Instability _ => new Action() { Type = "Instability" },
                Dynamics.Actions.Loan Loan => new Action() { Type = "Loan", Player = Loan.Debtor, Debt = Loan.Debt, Remaining = Loan.Remaining, Repayment = Loan.Repayment },
                Dynamics.Actions.Mortality _ => new Action() { Type = "Mortality" },
                Dynamics.Actions.PortRenewal _ => new Action() { Type = "PortRenewal" },
                Dynamics.Actions.Reinforcement Reinforcement => new Action() { Type = "Reinforcement", Province = Reinforcement.Province, Army = Army.FromArmy(Reinforcement.Army) },
                _ => throw new System.Exception("Invalid type of Action: " + action)
            };
        }
    }
}