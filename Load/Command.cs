using System.Collections.Generic;

namespace Imperit.Load
{
    public class Command : IConvertibleToWith<Dynamics.ICommand, (State.Settings, IReadOnlyList<State.Player>, State.Provinces)>
    {
        public string? Type { get; set; }
        public int? Player { get; set; }
        public int? Recipient { get; set; }
        public int? Province { get; set; }
        public int? To { get; set; }
        public Army? Army { get; set; }
        public Army? Defending { get; set; }
        public uint? Amount { get; set; }
        public uint? Debt { get; set; }
        public uint? Soldiers { get; set; }
        public Dynamics.ICommand Convert(int i, (State.Settings, IReadOnlyList<State.Player>, State.Provinces) arg)
        {
            (State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces) = arg;
            if (Type == "Commands.Attack")
                return new Dynamics.Commands.Attack(provinces, players[Player ?? 0], provinces[Province ?? 0], provinces[To ?? 0], Army!.Convert(i, (settings, players)));
            if (Type == "Commands.Donation")
                return new Dynamics.Commands.Donation(players[Player ?? 0], players[Recipient ?? 0], Amount ?? 0);
            if (Type == "Commands.Loan")
                return new Dynamics.Commands.Loan(settings, players[Player ?? 0], Amount ?? 0, Debt ?? 0);
            if (Type == "Commands.Purchase" && provinces[Province ?? 0] is State.Land L1)
                return new Dynamics.Commands.Purchase(settings, players[Player ?? 0], L1, provinces);
            if (Type == "Commands.Recruitment" && provinces[Province ?? 0] is State.Land L2)
                return new Dynamics.Commands.Recruitment(settings, players[Player ?? 0], L2, Soldiers ?? 0);
            if (Type == "Commands.Reinforcement")
                return new Dynamics.Commands.Reinforcement(provinces, players[Player ?? 0], provinces[Province ?? 0], provinces[To ?? 0], Army!.Convert(i, (settings, players)));
            throw new System.Exception("Invalid type of Command: " + Type);
        }
        public static Command FromCommand(Dynamics.ICommand evt)
        {
            if (evt is Dynamics.Commands.Attack Attack)
                return new Command() { Type = "Commands.Attack", Player = Attack.Player.Id, Province = Attack.From.Id, To = Attack.To.Id, Army = Army.FromArmy(Attack.Army) };
            if (evt is Dynamics.Commands.Donation Donation)
                return new Command() { Type = "Commands.Donation", Player = Donation.Player.Id, Recipient = Donation.Recipient.Id, Amount = Donation.Amount };
            if (evt is Dynamics.Commands.Loan Loan)
                return new Command() { Type = "Commands.Loan", Player = Loan.Player.Id, Amount = Loan.Amount, Debt = Loan.Debt };
            if (evt is Dynamics.Commands.Purchase Purchase)
                return new Command() { Type = "Commands.Purchase", Player = Purchase.Player.Id, Province = Purchase.Land.Id };
            if (evt is Dynamics.Commands.Recruitment Recruitment)
                return new Command() { Type = "Commands.Recruitment", Player = Recruitment.Player.Id, Province = Recruitment.Land.Id, Soldiers = Recruitment.Soldiers };
            if (evt is Dynamics.Commands.Reinforcement Reinforcement)
                return new Command() { Type = "Commands.Reinforcement", Player = Reinforcement.Player.Id, Province = Reinforcement.From.Id, To = Reinforcement.To.Id, Army = Army.FromArmy(Reinforcement.Army) };
            throw new System.Exception("Invalid type of Command: " + evt);
        }
    }
}