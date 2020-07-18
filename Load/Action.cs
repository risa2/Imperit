using System.Collections.Generic;

namespace Imperit.Load
{
    public class Action : IConvertibleToWith<Dynamics.IAction, (State.Settings, IReadOnlyList<State.Player>)>
    {
        public string? Type { get; set; }
        public int? Province { get; set; }
        public Army? Army { get; set; }
        public int? Player { get; set; }
        public int? Amount { get; set; }
        public uint? Debt { get; set; }
        public Dynamics.IAction Convert(int i, (State.Settings, IReadOnlyList<State.Player>) arg)
        {
            var (settings, _) = arg;
            return Type switch
            {
                "AddSoldiers" => new Dynamics.Actions.AddSoldiers(Province.Must(), Army!.Convert(i, arg)),
                "Attack" => new Dynamics.Actions.Attack(Province.Must(), Army!.Convert(i, arg)),
                "Earn" => new Dynamics.Actions.Earn(),
                "IncomeDecrease" => new Dynamics.Actions.IncomeChange(Player.Must(), Amount ?? 0),
                "Instability" => new Dynamics.Actions.Instability(),
                "Loan" => new Dynamics.Actions.Loan(Player.Must(), Debt ?? 0, settings),
                "Mortality" => new Dynamics.Actions.Mortality(),
                "PortRenewal" => new Dynamics.Actions.PortRenewal(),
                "Seizure" => new Dynamics.Actions.Seizure(Player.Must(), Debt ?? 0),
                _ => throw new System.Exception("Invalid type of Action: " + Type)
            };

        }
        public static Action FromAction(Dynamics.IAction action)
        {
            return action switch
            {
                Dynamics.Actions.AddSoldiers Add => new Action() { Type = "AddSoldiers", Province = Add.Province, Army = Army.FromArmy(Add.Army) },
                Dynamics.Actions.Attack Attack => new Action() { Type = "Attack", Province = Attack.Province, Army = Army.FromArmy(Attack.Army) },
                Dynamics.Actions.Earn _ => new Action() { Type = "Earn" },
                Dynamics.Actions.IncomeChange IncomeDecrease => new Action() { Type = "IncomeDecrease", Player = IncomeDecrease.Player, Amount = IncomeDecrease.Change },
                Dynamics.Actions.Instability _ => new Action() { Type = "Instability" },
                Dynamics.Actions.Loan Loan => new Action() { Type = "Loan", Player = Loan.Debtor, Debt = Loan.Debt },
                Dynamics.Actions.Mortality _ => new Action() { Type = "Mortality" },
                Dynamics.Actions.PortRenewal _ => new Action() { Type = "PortRenewal" },
                Dynamics.Actions.Seizure Seizure => new Action { Type = "Seizure", Player = Seizure.Player, Debt = Seizure.Amount },
                _ => throw new System.Exception("Invalid type of Action: " + action)
            };
        }
    }
}