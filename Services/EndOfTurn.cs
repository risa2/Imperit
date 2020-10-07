using Imperit.State;
using System.Linq;

namespace Imperit.Services
{
	public interface IEndOfTurn
	{
		bool NextTurn();
	}
	public class EndOfTurn : IEndOfTurn
	{
		readonly IPlayersLoader players;
		readonly IProvincesLoader pr;
		readonly IActionLoader actions;
		readonly IActivePlayer active;
		readonly IPowersLoader powers;
		public EndOfTurn(IPlayersLoader players, IProvincesLoader pr, IActionLoader actions, IActivePlayer active, IPowersLoader powers)
		{
			this.players = players;
			this.pr = pr;
			this.actions = actions;
			this.active = active;
			this.powers = powers;
		}
		int LivingHumans => players.Count(player => !(player is Robot) && !(player is Savage) && player.Alive);
		void End()
		{
			actions.EndOfTurn(active.Id);
			powers.Add(players);
			active.Next(players);
		}
		void AllRobotsActions()
		{
			while (players[active.Id] is Robot robot && LivingHumans > 0)
			{
				_ = actions.Add(robot.Think(pr));
				End();
			}
		}
		public bool NextTurn()
		{
			End();
			AllRobotsActions();
			players.Save();
			pr.Save();
			actions.Save();
			return LivingHumans > 0 && !powers.Last.MajorityReached;
		}
	}
}
