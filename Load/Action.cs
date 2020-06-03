using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<Action>? Actions { get; set; }
        public Dynamics.IAction Convert(int i, (State.Settings, IReadOnlyList<State.Player>, State.Provinces) arg)
        {
            (var settings, var players, var provinces) = arg;
            if (Province is int province)
            {
                if (Type == "Attack")
                    return new Dynamics.Actions.Attack(province, Army!.Convert(i, (settings, players)));
                if (Type == "Instability" && provinces[province] is State.Land)
                    return new Dynamics.Actions.Instability(province, Player);
                if (Type == "Reinforcement")
                    return new Dynamics.Actions.Reinforcement(province, Army!.Convert(i, (settings, players)));
                if (Type == "PortRenewal" && provinces[province] is State.Port port)
                    return new Dynamics.Actions.PortRenewal(province);
            }
            if (Player is int player)
            {
                if (Type == "Earn")
                    return new Dynamics.Actions.Earn(player);
                if (Type == "Mortality")
                    return new Dynamics.Actions.Mortality(player, provinces);
                if (Type == "Repayment")
                    return new Dynamics.Actions.Repayment(settings, player, Debt ?? 0, Remaining ?? 0);
                if (Type == "IncomeIncrease")
                    return new Dynamics.Actions.IncomeIncrease(player, Amount ?? 0);
                if (Type == "IncomeDecrease")
                    return new Dynamics.Actions.IncomeDecrease(player, Amount ?? 0);
            }
            if (Actions != null && Type == "Combination")
            {
                return new Dynamics.Actions.Combination(Actions.Select(action => action.Convert(i, arg)));
            }
            throw new System.Exception("Invalid type of Action: " + Type);
        }
        public static Action FromAction(Dynamics.IAction action)
        {
            if (action is Dynamics.Actions.Attack Attack)
                return new Action() { Type = "Attack", Province = Attack.Province, Army = Army.FromArmy(Attack.Army) };
            if (action is Dynamics.Actions.Earn Earn)
                return new Action() { Type = "Earn", Player = Earn.Player };
            if (action is Dynamics.Actions.Instability Instability)
                return new Action() { Type = "Instability", Player = Instability.LoyalTo, Province = Instability.Land };
            if (action is Dynamics.Actions.Mortality Mortality)
                return new Action() { Type = "Mortality", Player = Mortality.Player };
            if (action is Dynamics.Actions.PortRenewal PortRenewal)
                return new Action() { Type = "PortRenewal", Province = PortRenewal.Port };
            if (action is Dynamics.Actions.Reinforcement Reinforcement)
                return new Action() { Type = "Reinforcement", Province = Reinforcement.Province, Army = Army.FromArmy(Reinforcement.Army) };
            if (action is Dynamics.Actions.Repayment Repayment)
                return new Action() { Type = "Repayment", Player = Repayment.Debtor, Debt = Repayment.Debt, Remaining = Repayment.Remaining };
            if (action is Dynamics.Actions.IncomeIncrease IncomeIncrease)
                return new Action() { Type = "IncomeIncrease", Player = IncomeIncrease.Player, Amount = IncomeIncrease.Change };
            if (action is Dynamics.Actions.IncomeDecrease IncomeDecrease)
                return new Action() { Type = "IncomeDecrease", Player = IncomeDecrease.Player, Amount = IncomeDecrease.Change };
            if (action is Dynamics.Actions.Combination Combination)
                return new Action() { Type = "Combination", Actions = Combination.Actions.Select(action => FromAction(action)) };
            throw new System.Exception("Invalid type of Action: " + action);
        }
    }
}