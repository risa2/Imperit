using Imperit.Dynamics;
using Imperit.Dynamics.Commands;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Imperit.State
{
	public class Robot : Player
	{
		readonly Settings settings;
		readonly System.Random rand = new System.Random();
		public Robot(int id, string name, Color color, Password password, int money, bool alive, Settings settings)
			: base(id, name, color, password, money, alive) => this.settings = settings;
		public override Player ChangeMoney(int amount) => new Robot(Id, Name, Color, Password, Money + amount, Alive, settings);
		public override Player Die() => new Robot(Id, Name, Color, Password, 0, false, settings);
		class PInfo
		{
			public Soldiers Soldiers;
			public readonly bool Ally, Enemy;
			public int Enemies, Coming;
			public PInfo(Soldiers soldiers, bool ally, bool enemy, int enemies, int coming)
			{
				Soldiers = soldiers;
				Ally = ally;
				Enemy = enemy;
				Enemies = enemies;
				Coming = coming;
			}
			public int AttackPower => Soldiers.AttackPower;
			public int DefensePower => Soldiers.DefensePower;
			public int NextDefensePower => DefensePower + Coming;
			public int Bilance => ((Ally ? 1 : -1) * DefensePower) + Coming - Enemies;
		}
		IEnumerable<Province> NeighborEnemies(IProvinces provinces, Province prov) => provinces.NeighborsOf(prov.Id).Where(neighbor => !neighbor.IsAllyOf(Id) && neighbor.Occupied);
		int EnemiesCount(IProvinces provinces, Province prov) => NeighborEnemies(provinces, prov).Sum(neighbor => neighbor.Soldiers.AttackPower);
		void Recruit(List<ICommand> result, ref int spent, Province land, PInfo[] info, Soldiers soldiers)
		{
			info[land.Id].Coming += soldiers.DefensePower;
			result.Add(new Recruit(Id, land.Id, new Army(soldiers, this)));
			spent += soldiers.Price;
		}
		SoldierType? BestDefender(Province p, int money)
		{
			return settings.SoldierTypes.Where(type => type.IsRecruitable(p)).MinBy(type => -money / type.Price * type.DefensePower);
		}
		void DefensiveRecruits(List<ICommand> result, ref int spent, PInfo[] info, Province[] my)
		{
			foreach (Province l in my)
			{
				if (BestDefender(l, Money - spent) is SoldierType type && info[l.Id].Bilance < 0 && info[l.Id].Bilance + ((Money - spent) / type.Price * type.DefensePower) >= 0)
				{
					Recruit(result, ref spent, l, info, new Soldiers(type, (-info[l.Id].Bilance).DivUp(type.DefensePower)));
				}
			}
		}
		void StabilisatingRecruits(List<ICommand> result, ref int spent, PInfo[] info, Province[] my)
		{
			foreach (Province l in my)
			{
				if (Money <= spent)
				{
					break;
				}
				if (BestDefender(l, Money - spent) is SoldierType type && info[l.Id].NextDefensePower < l.DefaultSoldiers.DefensePower)
				{
					Recruit(result, ref spent, l, info, new Soldiers(type, Min((Money - spent) / type.Price, (l.DefaultSoldiers.DefensePower - info[l.Id].NextDefensePower).DivUp(type.DefensePower))));
				}
			}
		}
		void Recruits(List<ICommand> result, PInfo[] info, Province[] my)
		{
			int spent = 0;
			rand.Shuffle(my);
			DefensiveRecruits(result, ref spent, info, my);
			StabilisatingRecruits(result, ref spent, info, my);
			if (my.Any() && BestDefender(my[0], Money - spent) is SoldierType type && Money - spent > 0)
			{
				Recruit(result, ref spent, my[0], info, new Soldiers(type, (Money - spent) / type.Price));
			}
		}
		static bool CanConquer(Soldiers from, Soldiers to) => from.AttackPower > to.DefensePower;
		static bool CanKeepConqueredProvince(Soldiers from, Soldiers to, int enemies_to) => from.AttackPower >= to.DefensePower + enemies_to;
		static bool CanAttackSuccesfully(Soldiers attackers, Soldiers defenders, int enemies_to) => CanConquer(attackers, defenders) && CanKeepConqueredProvince(attackers, defenders, enemies_to);
		static bool ShouldAttack(Soldiers attackers, PInfo to) => !to.Ally && CanAttackSuccesfully(attackers, to.Soldiers, to.Enemies);
		void Attack(List<ICommand> result, IProvinces provinces, PInfo[] info, int from, int to, Soldiers soldiers)
		{
			result.Add(new Attack(Id, from, provinces[to], new Army(soldiers, this)));
			info[from].Soldiers -= soldiers;
			if (provinces[to].Occupied && soldiers.AttackPower >= provinces[to].Soldiers.DefensePower)
			{
				foreach (var neighbor in provinces.NeighborsOf(to).Where(n => n.IsAllyOf(Id)))
				{
					info[neighbor.Id].Enemies -= provinces[to].Soldiers.AttackPower;
				}
			}
		}
		static readonly Soldiers Unit = new Soldiers(new SoldierTypes.Pedestrian(0, new Description("", "", ""), 1, 1, 1, 1), 1);
		void Attacks(List<ICommand> result, IProvinces provinces, PInfo[] info, int[] my)
		{
			foreach (int from in my)
			{
				foreach (var (to, soldiers) in provinces.NeighborsOf(from).Select(to => (to.Id, info[from].Soldiers.MaxAttackers(provinces, from, to.Id).AttackedBy(Unit.Multiply(info[from].Enemies - ((info[to.Id].Enemy ? 1 : 0) * info[to.Id].AttackPower))))).Where(to => ShouldAttack(to.Item2, info[to.Id])))
				{
					Attack(result, provinces, info, from, to, soldiers);
				}
			}
		}
		IEnumerable<Province> NeighborAllies(IProvinces provinces, int i) => provinces.NeighborsOf(i).Where(n => n.IsAllyOf(Id));
		void MultiAttacks(List<ICommand> result, IProvinces provinces, PInfo[] info, int[] my)
		{
			foreach (int to in my.SelectMany(i => provinces.NeighborsOf(i).Where(p => p.Occupied && !p.IsAllyOf(Id)).Select(p => p.Id)).Distinct())
			{
				var starts = NeighborAllies(provinces, to);
				int total = starts.Aggregate(new Soldiers(), (s, from) => s.Add(from.Soldiers.MaxAttackers(provinces, from.Id, to).AttackedBy(Unit.Multiply(info[from.Id].Enemies - info[to].AttackPower)))).AttackPower;
				if (total > info[to].DefensePower + info[to].Enemies)
				{
					foreach (var from in starts)
					{
						Attack(result, provinces, info, from.Id, to, from.Soldiers.MaxAttackers(provinces, from.Id, to).AttackedBy(Unit.Multiply(info[from.Id].Enemies - info[to].AttackPower)));
					}
				}
			}
		}
		void Transport(List<ICommand> result, PInfo[] info, int from, Province to, Soldiers soldiers)
		{
			result.Add(new Reinforce(Id, from, to, new Army(soldiers, this)));
			info[from].Soldiers -= soldiers;
			info[to.Id].Coming += soldiers.DefensePower;
		}
		void SpreadSoldiers(List<ICommand> result, IProvinces provinces, PInfo[] info, int[] my)
		{
			foreach (int from in my)
			{
				foreach (var dest in provinces.NeighborsOf(from).Where(n => n.IsAllyOf(Id)).OrderBy(n => info[n.Id].Bilance))
				{
					if (info[from].Enemies <= 0 && info[dest.Id].Enemies > 0)
					{
						Transport(result, info, from, dest, info[from].Soldiers);
					}
					else if (info[from].Bilance > 0 && info[from].Bilance >= info[dest.Id].Bilance)
					{
						Transport(result, info, from, dest, info[from].Soldiers.AttackedBy(provinces[from].DefaultSoldiers));
					}
				}
			}
		}
		public List<ICommand> Think(IProvinces provinces)
		{
			var my = provinces.Where(p => p.IsAllyOf(Id)).Select(p => p.Id).ToArray();
			var info = provinces.Select(prov => new PInfo(prov.Soldiers, prov.IsAllyOf(Id), prov.Occupied && !prov.IsAllyOf(Id), EnemiesCount(provinces, prov), 0)).ToArray();

			var result = new List<ICommand>();
			Recruits(result, info, my.Select(i => provinces[i]).ToArray());
			Attacks(result, provinces, info, my);
			MultiAttacks(result, provinces, info, my);
			SpreadSoldiers(result, provinces, info, my);
			return result;
		}
	}
}
