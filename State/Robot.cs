using System.Collections.Generic;
using System.Linq;

namespace Imperit.State
{
    public class Robot : Player
    {
        public Robot(int id, string name, Color color, Password password, uint money, double credibility, bool alive, uint income) : base(id, name, color, password, money, credibility, alive, income) { }
        public override Player LoseCredibility(double amount) => new Robot(Id, Name, Color, Password, Money, NewCredibility(Credibility, -amount), Alive, Income);
        public override Player GainMoney(uint amount) => new Robot(Id, Name, Color, Password, Money + amount, Credibility, Alive, Income);
        public override Player Pay(uint amount) => new Robot(Id, Name, Color, Password, Money - amount, Credibility, Alive, Income);
        public override Player Die() => new Robot(Id, Name, Color, Password, 0, 1.0, false, 0);
        public override Player IncreaseIncome(uint change) => new Robot(Id, Name, Color, Password, Money, Credibility, Alive, Income + change);
        public override Player DecreaseIncome(uint change) => new Robot(Id, Name, Color, Password, Money, Credibility, Alive, Income - change);
        enum Relation { Enemy, Ally, Empty }
        class PInfo
        {
            public Relation Relation;
            public uint Soldiers, Enemies, Coming;
            public PInfo(uint soldiers, uint enemies, uint coming, Relation relation)
            {
                Soldiers = soldiers;
                Enemies = enemies;
                Coming = coming;
                Relation = relation;
            }
            public uint SoldiersNext => Soldiers + Coming;
            public int Bilance => (int)SoldiersNext - (int)Enemies;
        }
        static uint Min(uint a, uint b) => a > b ? b : a;
        static uint Max(uint a, uint b) => a > b ? a : b;
        IEnumerable<Province> NeighborEnemies(Provinces provinces, Province prov) => provinces.NeighborsOf(prov).Where(neighbor => !neighbor.IsControlledBy(this) && neighbor.Occupied);
        uint EnemiesCount(Provinces provinces, Province prov) => (uint)NeighborEnemies(provinces, prov).Sum(neighbor => neighbor.Soldiers);
        Relation GetRelationTo(Province prov) => prov.IsControlledBy(this) ? Relation.Ally : prov.Occupied ? Relation.Enemy : Relation.Empty;
        void Recruit(List<Dynamics.ICommand> result, ref uint spent, Land land, PInfo[] info, uint count)
        {
            info[land.Id].Coming += count;
            result.Add(new Dynamics.Commands.Recruitment(Id, land.Id, count));
            spent += count;
        }
        void DefensiveRecruitments(List<Dynamics.ICommand> result, ref uint spent, Provinces provinces, PInfo[] info, int[] my)
        {
            foreach (int i in my)
            {
                if (info[i].Bilance < 0 && info[i].Bilance + Money - spent >= 0 && provinces[i] is Land land)
                {
                    Recruit(result, ref spent, land, info, (uint)(-info[i].Bilance));
                }
            }
        }
        void StabilisatingRecruitments(List<Dynamics.ICommand> result, ref uint spent, Provinces provinces, PInfo[] info, int[] my)
        {
            foreach (int i in my)
            {
                if (Money == spent)
                {
                    break;
                }
                if (info[i].SoldiersNext < 80 && provinces[i] is Land land)
                {
                    Recruit(result, ref spent, land, info, Min(Money - spent, 80 - info[i].SoldiersNext));
                }
            }
        }
        void Recruitments(List<Dynamics.ICommand> result, Provinces provinces, PInfo[] info, int[] my)
        {
            uint spent = 0;
            DefensiveRecruitments(result, ref spent, provinces, info, my);
            StabilisatingRecruitments(result, ref spent, provinces, info, my);
            if (Money - spent > 0)
            {
                Recruit(result, ref spent, my.Select(i => provinces[i] as Land).NotNull().MinBy(prov => prov.Soldiers), info, Money - spent);
            }
        }
        static bool RevengeDoesNotMatter(Provinces provinces, int from, int to) => provinces.NeighborsOf(from).Concat(provinces.NeighborsOf(to)).All(n => !n.Occupied || n.IsAllyOf(provinces[to].Army) || n.IsAllyOf(provinces[from].Army));
        static bool CanConquerProvince(PInfo from, PInfo to) => from.Soldiers > to.Soldiers;
        static bool CanKeepConqueredProvince(PInfo from, PInfo to) => from.Soldiers >= to.Soldiers + to.Enemies;
        static bool CanKeepAttackStartProvinceAfterAttack(Provinces provinces, int from, int to, PInfo[] info) => info[from].Bilance > (RevengeDoesNotMatter(provinces, from, to) ? 0 : info[to].Enemies) + (info[to].Relation == Relation.Empty ? 0 : info[to].Soldiers);
        static bool CanAttackSuccesfully(Provinces provinces, int from, int to, PInfo[] info) => CanConquerProvince(info[from], info[to]) && (CanKeepConqueredProvince(info[from], info[to]) || RevengeDoesNotMatter(provinces, from, to)) && CanKeepAttackStartProvinceAfterAttack(provinces, from, to, info);
        bool ShouldAttack(Provinces provinces, int from, int to, PInfo[] info) => info[to].Relation != Relation.Ally && CanAttackSuccesfully(provinces, from, to, info);
        void Attack(List<Dynamics.ICommand> result, Settings settings, Provinces provinces, PInfo[] info, int from, int to, uint count)
        {
            result.Add(new Dynamics.Commands.Attack(Id, from, to, new PlayerArmy(settings, this, count)));
            info[from].Soldiers -= count;
            if (provinces[from].Occupied && count >= provinces[to].Soldiers)
            {
                foreach (var neighbor in provinces.NeighborsOf(provinces[to]).Where(n => n.IsControlledBy(this)))
                {
                    info[neighbor.Id].Enemies -= provinces[to].Soldiers;
                }
            }
        }
        void Attacks(List<Dynamics.ICommand> result, Settings settings, Provinces provinces, PInfo[] info, int[] my)
        {
            foreach (int from in my)
            {
                foreach (var to in provinces.NeighborsOf(provinces[from]).Where(to => ShouldAttack(provinces, from, to.Id, info)))
                {
                    Attack(result, settings, provinces, info, from, to.Id, Min(Min(info[from].Soldiers, provinces[from].CanMoveTo(to)), to.Soldiers + info[to.Id].Enemies + 1));
                }
            }
        }
        static uint MultiAttackSoldiers(Provinces provinces, PInfo[] info, int from, int to) => Min((uint)info[from].Bilance + provinces[to].Soldiers, info[from].Soldiers);
        void MultiAttacks(List<Dynamics.ICommand> result, Settings settings, Provinces provinces, PInfo[] info, int[] my)
        {
            foreach (int to in my.SelectMany(i => provinces.NeighborsOf(provinces[i]).Where(p => p.Occupied && !p.IsControlledBy(this)).Select(p => p.Id)).Distinct())
            {
                var starts = provinces.NeighborsOf(to).Where(n => n.IsControlledBy(this) && info[n.Id].Bilance + provinces[to].Soldiers > 0);
                uint bilance = (uint)starts.Sum(n => MultiAttackSoldiers(provinces, info, n.Id, to));
                if (bilance > info[to].Soldiers + (starts.All(n => RevengeDoesNotMatter(provinces, n.Id, to)) ? 0 : info[to].Enemies))
                {
                    foreach (var from in starts)
                    {
                        Attack(result, settings, provinces, info, from.Id, to, MultiAttackSoldiers(provinces, info, from.Id, to));
                    }
                }
            }
        }
        void Transport(List<Dynamics.ICommand> result, Settings settings, PInfo[] info, int from, int to, uint count)
        {
            result.Add(new Dynamics.Commands.Reinforcement(Id, from, to, new PlayerArmy(settings, this, count)));
            info[from].Soldiers -= count;
            info[to].Coming += count;
        }
        void SpreadSoldiers(List<Dynamics.ICommand> result, Settings settings, Provinces provinces, PInfo[] info, int[] my)
        {
            foreach (int from in my)
            {
                foreach (var dest in provinces.NeighborsOf(provinces[from]).Where(n => n.IsControlledBy(this)).OrderBy(n => info[n.Id].Bilance))
                {
                    if (info[from].Enemies <= 0 && info[dest.Id].Enemies > 0)
                    {
                        Transport(result, settings, info, from, dest.Id, info[from].Soldiers);
                    }
                    else if (info[from].Enemies <= 0 && info[dest.Id].Enemies <= 0)
                    {
                        Transport(result, settings, info, from, dest.Id, info[from].Soldiers / Max(provinces.NeighborCount(provinces[from]) - 1, 1));
                    }
                    else if (info[from].Bilance > 0 && info[from].Bilance >= info[dest.Id].Bilance)
                    {
                        Transport(result, settings, info, from, dest.Id, Min((uint)info[from].Bilance, info[from].Soldiers));
                    }
                }
            }
        }
        public List<Dynamics.ICommand> Think(Settings settings, Provinces provinces)
        {
            var my = provinces.Where(p => p.IsControlledBy(this)).Select(p => p.Id).ToArray();
            var info = provinces.Select(prov => new PInfo(prov.Soldiers, EnemiesCount(provinces, prov), 0, GetRelationTo(prov))).ToArray();

            var result = new List<Dynamics.ICommand>();
            Recruitments(result, provinces, info, my);
            Attacks(result, settings, provinces, info, my);
            MultiAttacks(result, settings, provinces, info, my);
            SpreadSoldiers(result, settings, provinces, info, my);
            return result;
        }
    }
}
