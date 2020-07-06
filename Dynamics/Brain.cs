using Imperit.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Imperit.Dynamics
{
    public class Brain
    {
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
            public int Bilance => (int)(SoldiersNext - Enemies);
        }
        State.Player player;
        public Brain(State.Player player) => this.player = player;
        IEnumerable<State.Province> NeighborEnemies(State.Provinces provinces, State.Province prov) => provinces.NeighborsOf(prov).Where(neighbor => !neighbor.IsControlledBy(player) && neighbor.Occupied);
        uint EnemiesCount(State.Provinces provinces, State.Province prov) => (uint)NeighborEnemies(provinces, prov).Sum(neighbor => neighbor.Soldiers);
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
                if (info[i].SoldiersNext < 65 && provinces[i] is State.Land land)
                {
                    Recruit(result, land, info, (uint)Min(player.Money, 75 - info[i].SoldiersNext));
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
        static bool CanAttackSuccesfully(PInfo from, PInfo to) => from.Soldiers > to.Soldiers + to.Enemies && from.Bilance > to.Enemies + (to.Relation == Relation.Enemy ? 0 : to.Soldiers);
        static bool CanAttackDesperately(PInfo from, PInfo to) => from.Enemies < from.Soldiers + from.Coming && to.Relation == Relation.Enemy;
        bool ShouldAttack(PInfo from, PInfo to) => to.Relation != Relation.Ally && (CanAttackDesperately(from, to) || CanAttackSuccesfully(from, to));
        void Attack(List<ICommand> result, State.Settings settings, State.Provinces provinces, PInfo[] info, int from, State.Province to, uint count)
        {
            result.Add(new Commands.Attack(player.Id, from, to.Id, new State.PlayerArmy(settings, player, count)));
            info[from].Soldiers -= count;
            if (provinces[from].Occupied && count >= to.Soldiers)
            {
                foreach (var neighbor in provinces.NeighborsOf(provinces[to.Id]).Where(n => n.IsControlledBy(player)))
                {
                    info[neighbor.Id].Enemies -= to.Soldiers;
                }
            }
        }
        List<ICommand> Attacks(State.Settings settings, State.Provinces provinces, PInfo[] info, int[] my)
        {
            var result = new List<ICommand>();
            foreach (int from in my)
            {
                foreach (var to in provinces.NeighborsOf(provinces[from]).Where(to => ShouldAttack(info[from], info[to.Id])))
                {
                    Attack(result, settings, provinces, info, from, to, Min(Min(info[from].Soldiers, provinces[from].CanMoveTo(to)), to.Soldiers + info[to.Id].Enemies + 1));
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
                        Transport(result, settings, info, from, dest.Id, info[from].Soldiers / (uint)Max(provinces.NeighborCount(provinces[from]) - 1, 1));
                    }
                    else if (info[from].Bilance > 0 && info[from].Bilance >= info[dest.Id].Bilance)
                    {
                        Transport(result, settings, info, from, dest.Id, (uint)Min(info[from].Bilance, info[from].Soldiers));
                    }
                }
            }
            return result;
        }
        public IEnumerable<ICommand> Think(State.Settings settings, State.Provinces provinces)
        {
            var my = provinces.Where(p => p.IsControlledBy(player)).Select(p => p.Id).ToArray();
            var info = provinces.Select(prov => new PInfo(prov.Soldiers, EnemiesCount(provinces, prov), 0, prov.IsControlledBy(player) ? Relation.Ally : prov.Occupied ? Relation.Enemy : Relation.Empty)).ToArray();

            var result = Recruitments(provinces, info, my);
            result = result.Concat(Attacks(settings, provinces, info, my));
            result = result.Concat(SpreadSoldiers(settings, provinces, info, my));
            return result;
        }
    }
}