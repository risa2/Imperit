using System.Collections.Generic;
using System.Collections.Immutable;
//using System.Linq;
//using Imperit.Dynamics.Commands;

namespace Imperit.State
{
	public class Robot : Player
	{
		public Robot(int id, string name, Color color, Password password, int money, bool alive, ImmutableArray<SoldierType> types)
			: base(id, name, color, password, money, alive, types) { }
		public override Player ChangeMoney(int amount) => new Robot(Id, Name, Color, Password, Money + amount, Alive, SoldierTypes);
		public override Player Die() => new Robot(Id, Name, Color, Password, 0, false, ImmutableArray<SoldierType>.Empty);
		public override Player AddSoldierTypes(params SoldierType[] types) => new Robot(Id, Name, Color, Password, Money, Alive, SoldierTypes.AddRange(types));
		//enum Relation { Enemy, Ally, Empty }
		//class PInfo
		//{
		//    public Relation Relation;
		//    public int Soldiers, Enemies, Coming;
		//    public PInfo(int soldiers, int enemies, int coming, Relation relation)
		//    {
		//        Soldiers = soldiers;
		//        Enemies = enemies;
		//        Coming = coming;
		//        Relation = relation;
		//    }
		//    public int SoldiersNext => Soldiers + Coming;
		//    public int Bilance => SoldiersNext - Enemies;
		//}
		//static int Min(int a, int b) => a > b ? b : a;
		//static int Max(int a, int b) => a > b ? a : b;
		//IEnumerable<Province> NeighborEnemies(IProvinces provinces, Province prov) => provinces.NeighborsOf(prov.Id).Where(neighbor => !neighbor.IsAllyOf(Id) && neighbor.Occupied);
		//int EnemiesCount(IProvinces provinces, Province prov) => NeighborEnemies(provinces, prov).Sum(neighbor => neighbor.Soldiers);
		//Relation GetRelationTo(Province prov) => prov.IsAllyOf(Id) ? Relation.Ally : prov.Occupied ? Relation.Enemy : Relation.Empty;
		//void Recruit(List<Dynamics.ICommand> result, ref int spent, Land land, PInfo[] info, int count)
		//{
		//    info[land.Id].Coming += count;
		//    result.Add(new Recruitment(Id, land.Id, new PlayerArmy(this, count)));
		//    spent += count;
		//}
		//void DefensiveRecruitments(List<Dynamics.ICommand> result, ref int spent, PInfo[] info, Land[] my)
		//{
		//    foreach (Land l in my)
		//    {
		//        if (info[l.Id].Bilance < 0 && info[l.Id].Bilance + Money - spent >= 0)
		//        {
		//            Recruit(result, ref spent, l, info, -info[l.Id].Bilance);
		//        }
		//    }
		//}
		//void StabilisatingRecruitments(List<Dynamics.ICommand> result, ref int spent, PInfo[] info, Land[] my)
		//{
		//    foreach (Land l in my)
		//    {
		//        if (Money == spent)
		//        {
		//            break;
		//        }
		//        if (info[l.Id].SoldiersNext < 80)
		//        {
		//            Recruit(result, ref spent, l, info, Min(Money - spent, 80 - info[l.Id].SoldiersNext));
		//        }
		//    }
		//}
		//void Recruitments(List<Dynamics.ICommand> result, IProvinces provinces, PInfo[] info, int[] my)
		//{
		//    int spent = 0;
		//    var lands = my.Select(i => provinces[i] as Land).NotNull().ToArray();
		//    DefensiveRecruitments(result, ref spent, info, lands);
		//    StabilisatingRecruitments(result, ref spent, info, lands);
		//    if (Money - spent > 0 && lands.Any())
		//    {
		//        Recruit(result, ref spent, lands.MinBy(prov => prov.Soldiers), info, Money - spent);
		//    }
		//}
		//static bool RevengeDoesNotMatter(IProvinces provinces, int from, int to) => provinces.NeighborsOf(from).Concat(provinces.NeighborsOf(to)).All(n => !n.Occupied || n.IsAllyOf(provinces[to]) || n.IsAllyOf(provinces[from]));
		//static bool CanConquerProvince(PInfo from, PInfo to) => from.Soldiers > to.Soldiers;
		//static bool CanKeepConqueredProvince(PInfo from, PInfo to) => from.Soldiers >= to.Soldiers + to.Enemies;
		//static bool CanKeepAttackStartProvinceAfterAttack(IProvinces provinces, int from, int to, PInfo[] info) => info[from].Bilance > (RevengeDoesNotMatter(provinces, from, to) ? 0 : info[to].Enemies) + (info[to].Relation == Relation.Empty ? 0 : info[to].Soldiers);
		//static bool CanAttackSuccesfully(IProvinces provinces, int from, int to, PInfo[] info) => CanConquerProvince(info[from], info[to]) && (CanKeepConqueredProvince(info[from], info[to]) || RevengeDoesNotMatter(provinces, from, to)) && CanKeepAttackStartProvinceAfterAttack(provinces, from, to, info);
		//static bool ShouldAttack(IProvinces provinces, int from, int to, PInfo[] info) => info[to].Relation != Relation.Ally && CanAttackSuccesfully(provinces, from, to, info);
		//void Attack(List<Dynamics.ICommand> result, IProvinces provinces, PInfo[] info, int from, int to, int count)
		//{
		//    result.Add(new Attack(Id, from, provinces[to], new PlayerArmy(this, count)));
		//    info[from].Soldiers -= count;
		//    if (provinces[from].Occupied && count >= provinces[to].Soldiers)
		//    {
		//        foreach (var neighbor in provinces.NeighborsOf(to).Where(n => n.IsAllyOf(Id)))
		//        {
		//            info[neighbor.Id].Enemies -= provinces[to].Soldiers;
		//        }
		//    }
		//}
		//void Attacks(List<Dynamics.ICommand> result, IProvinces provinces, PInfo[] info, int[] my)
		//{
		//    foreach (int from in my)
		//    {
		//        foreach (var to in provinces.NeighborsOf(from).Where(to => ShouldAttack(provinces, from, to.Id, info)))
		//        {
		//            Attack(result, provinces, info, from, to.Id, Min(Min(info[from].Soldiers, provinces[from].CanMoveTo(to)), to.Soldiers + info[to.Id].Enemies + 1));
		//        }
		//    }
		//}
		//static int MultiAttackSoldiers(IProvinces provinces, PInfo[] info, int from, int to) => Min(info[from].Bilance + provinces[to].Soldiers, info[from].Soldiers);
		//void MultiAttacks(List<Dynamics.ICommand> result, IProvinces provinces, PInfo[] info, int[] my)
		//{
		//    foreach (int to in my.SelectMany(i => provinces.NeighborsOf(i).Where(p => p.Occupied && !p.IsAllyOf(Id)).Select(p => p.Id)).Distinct())
		//    {
		//        var starts = provinces.NeighborsOf(to).Where(n => n.IsAllyOf(Id) && info[n.Id].Bilance + provinces[to].Soldiers > 0);
		//        int bilance = starts.Sum(n => MultiAttackSoldiers(provinces, info, n.Id, to));
		//        if (bilance > info[to].Soldiers + (starts.All(n => RevengeDoesNotMatter(provinces, n.Id, to)) ? 0 : info[to].Enemies))
		//        {
		//            foreach (var from in starts)
		//            {
		//                Attack(result, provinces, info, from.Id, to, MultiAttackSoldiers(provinces, info, from.Id, to));
		//            }
		//        }
		//    }
		//}
		//void Transport(List<Dynamics.ICommand> result, PInfo[] info, int from, Province to, int count)
		//{
		//    result.Add(new Reinforcement(Id, from, to, new PlayerArmy(this, count)));
		//    info[from].Soldiers -= count;
		//    info[to.Id].Coming += count;
		//}
		//void SpreadSoldiers(List<Dynamics.ICommand> result, IProvinces provinces, PInfo[] info, int[] my)
		//{
		//    foreach (int from in my)
		//    {
		//        foreach (var dest in provinces.NeighborsOf(from).Where(n => n.IsAllyOf(Id)).OrderBy(n => info[n.Id].Bilance))
		//        {
		//            if (info[from].Enemies <= 0 && info[dest.Id].Enemies > 0)
		//            {
		//                Transport(result, info, from, dest, info[from].Soldiers);
		//            }
		//            else if (info[from].Enemies <= 0 && info[dest.Id].Enemies <= 0)
		//            {
		//                Transport(result, info, from, dest, info[from].Soldiers / Max(provinces.NeighborCount(from) - 1, 1));
		//            }
		//            else if (info[from].Bilance > 0 && info[from].Bilance >= info[dest.Id].Bilance)
		//            {
		//                Transport(result, info, from, dest, Min(info[from].Bilance, info[from].Soldiers));
		//            }
		//        }
		//    }
		//}
		public List<Dynamics.ICommand> Think(IProvinces provinces)
		{
			//var my = provinces.Where(p => p.IsAllyOf(Id)).Select(p => p.Id).ToArray();
			//var info = provinces.Select(prov => new PInfo(prov.Soldiers, EnemiesCount(provinces, prov), 0, GetRelationTo(prov))).ToArray();

			//var result = new List<Dynamics.ICommand>();
			//Recruitments(result, provinces, info, my);
			//Attacks(result, provinces, info, my);
			//MultiAttacks(result, provinces, info, my);
			//SpreadSoldiers(result, provinces, info, my);
			//return result;
			return Money + provinces.Count > 0 ? new List<Dynamics.ICommand>() : new List<Dynamics.ICommand>();
		}
	}
}
