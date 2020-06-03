using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Imperit.Dynamics
{
    public class Brain
    {
        private enum Relation { Enemy, Ally, Empty }
        private struct PInfo
        {
            public Relation Relation;
            public uint Soldiers, Enemies, Coming;
            public PInfo(uint soldiers = 0, uint enemies = 0, uint coming = 0, Relation relation = Relation.Empty)
            {
                Soldiers = soldiers;
                Enemies = enemies;
                Coming = coming;
                Relation = relation;
            }
            public uint SoldiersNext => Soldiers + Coming;
            public int Bilance => (int)(SoldiersNext - Enemies);
        }
        static System.Random rand = new System.Random();
        State.Player player;
        public Brain(State.Player player) => this.player = player;
        IEnumerable<State.Province> NeighborEnemies(State.Provinces provinces, State.Province prov) => provinces.NeighborsOf(prov).Where(neighbor => !neighbor.IsControlledBy(player) && neighbor.Occupied);
        uint EnemiesCount(State.Provinces provinces, State.Province prov) => (uint)NeighborEnemies(provinces, prov).Sum(neighbor => neighbor.Soldiers);
        void Recruit(List<ICommand> result, State.Settings settings, State.Land land, PInfo[] info, uint recruited)
        {
            info[land.Id].Coming += recruited;
            result.Add(new Commands.Recruitment(settings, player, land, recruited));
            player = player.Pay(recruited);
        }
        List<ICommand> DefensiveRecruitments(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int i in my)
            {
                if (info[i].Bilance < 0 && info[i].Bilance + player.Money >= 0 && provinces[i] is State.Land land)
                {
                    Recruit(result, settings, land, info, (uint)(-info[i].Bilance));
                }
            }
            return result;
        }
        List<ICommand> StabilisatingRecruitments(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int i in my)
            {
                if (player.Money == 0)
                {
                    break;
                }
                if (info[i].SoldiersNext < 65 && provinces[i] is State.Land land)
                {
                    Recruit(result, settings, land, info, (uint)Min(player.Money, 75 - info[i].SoldiersNext));
                }
            }
            return result;
        }
        IEnumerable<ICommand> Recruitments(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result1 = DefensiveRecruitments(settings, players, provinces, info, my);
            var result2 = StabilisatingRecruitments(settings, players, provinces, info, my);
            if (player.Money > 0)
            {
                Recruit(result2, settings, my.Select(i => provinces[i]).Casted<State.Land, State.Province>().MinBy(prov => prov.Soldiers), info, player.Money);
            }
            return result1.Concat(result2);
        }
        static bool CanAttackSuccesfully(State.Settings settings, State.Provinces provinces, PInfo from, PInfo to) => from.Soldiers > to.Soldiers + to.Enemies && from.Bilance > to.Enemies + (to.Relation == Relation.Enemy ? 0 : to.Soldiers);
        static bool CanAttackDesperately(State.Settings settings, State.Provinces provinces, PInfo from, PInfo to) => from.Enemies < from.Soldiers + from.Coming && to.Relation == Relation.Enemy;
        bool ShouldAttack(State.Settings settings, State.Provinces provinces, PInfo from, PInfo to) => to.Relation != Relation.Ally && (CanAttackDesperately(settings, provinces, from, to) || CanAttackSuccesfully(settings, provinces, from, to));
        List<ICommand> Attacks(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int from in my)
            {
                foreach (var to in provinces.NeighborsOf(provinces[from]).Where(to => ShouldAttack(settings, provinces, info[from], info[to.Id])))
                {
                    var attackers = Min(Min(info[from].Soldiers, provinces[from].CanMoveTo(to)), to.Soldiers + info[to.Id].Enemies + 1);
                    result.Add(new Commands.Attack(provinces, player, provinces[from], to, new State.PlayerArmy(settings, player, attackers)));
                    info[from].Soldiers -= attackers;
                    if (provinces[from].Occupied && attackers >= to.Soldiers)
                    {
                        foreach (var neighbor in provinces.NeighborsOf(provinces[to.Id]).Where(n => n.IsControlledBy(player)))
                        {
                            info[neighbor.Id].Enemies -= to.Soldiers;
                        }
                    }
                }
            }
            return result;
        }
        void Transport(List<ICommand> result, State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces, PInfo[] info, int from, int to, uint count)
        {
            result.Add(new Commands.Reinforcement(provinces, player, provinces[from], provinces[to], new State.PlayerArmy(settings, player, count)));
            info[from].Soldiers -= count;
            info[to].Coming += count;
        }
        List<ICommand> SpreadSoldiers(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int from in my)
            {
                foreach (var dest in provinces.NeighborsOf(provinces[from]).Where(n => n.IsControlledBy(player)).OrderBy(n => info[n.Id].Bilance))
                {
                    if (info[from].Enemies <= 0 && info[dest.Id].Enemies > 0)
                    {
                        Transport(result, settings, players, provinces, info, from, dest.Id, info[from].Soldiers);
                    }
                    else if (info[from].Enemies <= 0 && info[dest.Id].Enemies <= 0)
                    {
                        Transport(result, settings, players, provinces, info, from, dest.Id, info[from].Soldiers / (uint)Max(provinces.NeighborCount(provinces[from]) - 1, 1));
                    }
                    else if (info[from].Bilance > 0 && info[from].Bilance >= info[dest.Id].Bilance)
                    {
                        Transport(result, settings, players, provinces, info, from, dest.Id, (uint)Min(info[from].Bilance, info[from].Soldiers));
                    }
                }
            }
            return result;
        }
        public IEnumerable<ICommand> Think(State.Settings settings, IReadOnlyList<State.Player> players, State.Provinces provinces)
        {
            var my = provinces.Count.Range().Where(i => provinces[i].IsControlledBy(player)).ToArray();
            var info = provinces.Select(prov => new PInfo(prov.Soldiers, EnemiesCount(provinces, prov), 0, prov.IsControlledBy(player) ? Relation.Ally : prov.Occupied ? Relation.Enemy : Relation.Empty)).ToArray();

            var result = Recruitments(settings, players, provinces, info, my);
            result = result.Concat(Attacks(settings, players, provinces, info, my));
            result = result.Concat(SpreadSoldiers(settings, players, provinces, info, my));
            return result;
        }
    }
}