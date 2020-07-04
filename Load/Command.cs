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
        public uint? Repayment { get; set; }
        public Dynamics.ICommand Convert(int i, (State.Settings, IReadOnlyList<State.Player>, State.Provinces) arg)
        {
            (State.Settings settings, IReadOnlyList<State.Player> players, _) = arg;
            if (Type == "Commands.Attack")
                return new Dynamics.Commands.Attack(Player.Must(), Province.Must(), To.Must(), Army!.Convert(i, (settings, players)));
            if (Type == "Commands.Donation")
                return new Dynamics.Commands.Donation(Player.Must(), Recipient.Must(), Amount.Must());
            if (Type == "Commands.Loan")
                return new Dynamics.Commands.Loan(Player.Must(), Amount.Must(), Debt.Must(), Repayment.Must());
            if (Type == "Commands.Purchase")
                return new Dynamics.Commands.Purchase(Player.Must(), Province.Must());
            if (Type == "Commands.Recruitment")
                return new Dynamics.Commands.Recruitment(Player.Must(), Province.Must(), Soldiers.Must());
            if (Type == "Commands.Reinforcement")
                return new Dynamics.Commands.Reinforcement(Player.Must(), Province.Must(), To.Must(), Army!.Convert(i, (settings, players)));
            throw new System.Exception("Invalid type of Command: " + Type);
        }
        public static Command FromCommand(Dynamics.ICommand evt)
        {
            if (evt is Dynamics.Commands.Attack Attack)
                return new Command() { Type = "Commands.Attack", Player = Attack.Player, Province = Attack.From, To = Attack.To, Army = Army.FromArmy(Attack.Army) };
            if (evt is Dynamics.Commands.Donation Donation)
                return new Command() { Type = "Commands.Donation", Player = Donation.Player, Recipient = Donation.Recipient, Amount = Donation.Amount };
            if (evt is Dynamics.Commands.Loan Loan)
                return new Command() { Type = "Commands.Loan", Player = Loan.Player, Amount = Loan.Amount, Debt = Loan.Debt, Repayment = Loan.Repayment };
            if (evt is Dynamics.Commands.Purchase Purchase)
                return new Command() { Type = "Commands.Purchase", Player = Purchase.Player, Province = Purchase.Land };
            if (evt is Dynamics.Commands.Recruitment Recruitment)
                return new Command() { Type = "Commands.Recruitment", Player = Recruitment.Player, Province = Recruitment.Land, Soldiers = Recruitment.Soldiers };
            if (evt is Dynamics.Commands.Reinforcement Reinforcement)
                return new Command() { Type = "Commands.Reinforcement", Player = Reinforcement.Player, Province = Reinforcement.From, To = Reinforcement.To, Army = Army.FromArmy(Reinforcement.Army) };
            throw new System.Exception("Invalid type of Command: " + evt);
        }
    }
}