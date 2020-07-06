using Imperit.Load;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Dynamics
{
    public class Brain
    {
        State.Player player;
        public Brain(State.Player player) => this.player = player;
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
        IEnumerable<State.Province> NeighborEnemies(State.Provinces provinces, State.Province prov) => provinces.NeighborsOf(prov).Where(neighbor => !neighbor.IsControlledBy(player) && neighbor.Occupied);
        uint EnemiesCount(State.Provinces provinces, State.Province prov) => (uint)NeighborEnemies(provinces, prov).Sum(neighbor => neighbor.Soldiers);
        Relation GetRelationTo(State.Province prov) => prov.IsControlledBy(player) ? Relation.Ally : prov.Occupied ? Relation.Enemy : Relation.Empty;
        void Recruit(List<ICommand> result, State.Land land, PInfo[] info, uint recruited)
        {
            info[land.Id].Coming += recruited;
            result.Add(new Commands.Recruitment(player.Id, land.Id, recruited));
            player = player.Pay(recruited);
        }
        List<ICommand> DefensiveRecruitments(State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int i in my)
            {
                if (info[i].Bilance < 0 && info[i].Bilance + player.Money >= 0 && provinces[i] is State.Land land)
                {
                    Recruit(result, land, info, (uint)(-info[i].Bilance));
                }
            }
            return result;
        }
        List<ICommand> StabilisatingRecruitments(State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int i in my)
            {
                if (player.Money == 0)
                {
                    break;
                }
                if (info[i].SoldiersNext < 80 && provinces[i] is State.Land land)
                {
                    Recruit(result, land, info, Min(player.Money, 80 - info[i].SoldiersNext));
                }
            }
            return result;
        }
        IEnumerable<ICommand> Recruitments(State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result1 = DefensiveRecruitments(provinces, info, my);
            var result2 = StabilisatingRecruitments(provinces, info, my);
            if (player.Money > 0)
            {
                Recruit(result2, my.Select(i => provinces[i]).Casted<State.Land, State.Province>().MinBy(prov => prov.Soldiers), info, player.Money);
            }
            return result1.Concat(result2);
        }
        static bool RevengeDoesNotMatter(State.Provinces provinces, int from, int to) => provinces.NeighborsOf(from).Concat(provinces.NeighborsOf(to)).All(n => !n.Occupied || n.IsAllyOf(provinces[to].Army) || n.IsAllyOf(provinces[from].Army));
        static bool CanConquerProvince(PInfo from, PInfo to) => from.Soldiers > to.Soldiers;
        static bool CanKeepConqueredProvince(PInfo from, PInfo to) => from.Soldiers >= to.Soldiers + to.Enemies;
        static bool CanKeepAttackStartProvinceAfterAttack(State.Provinces provinces, int from, int to, PInfo[] info) => info[from].Bilance > (RevengeDoesNotMatter(provinces, from, to) ? 0 : info[to].Enemies) + (info[to].Relation == Relation.Empty ? 0 : info[to].Soldiers);
        static bool CanAttackSuccesfully(State.Provinces provinces, int from, int to, PInfo[] info) => CanConquerProvince(info[from], info[to]) && (CanKeepConqueredProvince(info[from], info[to]) || RevengeDoesNotMatter(provinces, from, to)) && CanKeepAttackStartProvinceAfterAttack(provinces, from, to, info);
        bool ShouldAttack(State.Provinces provinces, int from, int to, PInfo[] info) => info[to].Relation != Relation.Ally && CanAttackSuccesfully(provinces, from, to, info);
        void Attack(List<ICommand> result, State.Settings settings, State.Provinces provinces, PInfo[] info, int from, int to, uint count)
        {
            result.Add(new Commands.Attack(player.Id, from, to, new State.PlayerArmy(settings, player, count)));
            info[from].Soldiers -= count;
            if (provinces[from].Occupied && count >= provinces[to].Soldiers)
            {
                foreach (var neighbor in provinces.NeighborsOf(provinces[to]).Where(n => n.IsControlledBy(player)))
                {
                    info[neighbor.Id].Enemies -= provinces[to].Soldiers;
                }
            }
        }
        List<ICommand> Attacks(State.Settings settings, State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int from in my)
            {
                foreach (var to in provinces.NeighborsOf(provinces[from]).Where(to => ShouldAttack(provinces, from, to.Id, info)))
                {
                    Attack(result, settings, provinces, info, from, to.Id, Min(Min(info[from].Soldiers, provinces[from].CanMoveTo(to)), to.Soldiers + info[to.Id].Enemies + 1));
                }
            }
            return result;
        }
        static uint MultiAttackSoldiers(State.Provinces provinces, PInfo[] info, int from, int to) => Min((uint)info[from].Bilance + provinces[to].Soldiers, info[from].Soldiers);
        List<ICommand> MultiAttacks(State.Settings settings, State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int to in my.SelectMany(i => provinces.NeighborsOf(provinces[i]).Where(p => p.Occupied && !p.IsControlledBy(player)).Select(p => p.Id)).Distinct())
            {
                var starts = provinces.NeighborsOf(to).Where(n => n.IsControlledBy(player) && info[n.Id].Bilance + provinces[to].Soldiers > 0);
                uint bilance = (uint)starts.Sum(n => MultiAttackSoldiers(provinces, info, n.Id, to));
                if (bilance > info[to].Soldiers + (starts.All(n => RevengeDoesNotMatter(provinces, n.Id, to)) ? 0 : info[to].Enemies))
                {
                    foreach (var from in starts)
                    {
                        Attack(result, settings, provinces, info, from.Id, to, MultiAttackSoldiers(provinces, info, from.Id, to));
                    }
                }
            }
            return result;
        }
        void Transport(List<ICommand> result, State.Settings settings, PInfo[] info, int from, int to, uint count)
        {
            result.Add(new Commands.Reinforcement(player.Id, from, to, new State.PlayerArmy(settings, player, count)));
            info[from].Soldiers -= count;
            info[to].Coming += count;
        }
        List<ICommand> SpreadSoldiers(State.Settings settings, State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int from in my)
            {
                foreach (var dest in provinces.NeighborsOf(provinces[from]).Where(n => n.IsControlledBy(player)).OrderBy(n => info[n.Id].Bilance))
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
            return result;
        }
        public IEnumerable<ICommand> Think(State.Settings settings, State.Provinces provinces)
        {
            var my = provinces.Where(p => p.IsControlledBy(player)).Select(p => p.Id).ToArray();
            var info = provinces.Select(prov => new PInfo(prov.Soldiers, EnemiesCount(provinces, prov), 0, GetRelationTo(prov))).ToArray();

            var result = Recruitments(provinces, info, my);
            result = result.Concat(Attacks(settings, provinces, info, my));
            result = result.Concat(MultiAttacks(settings, provinces, info, my));
            result = result.Concat(SpreadSoldiers(settings, provinces, info, my));
            return result;
        }
    }
}