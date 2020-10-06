using Imperit.State;
using System.Linq;

namespace Imperit.Services
{
	public interface IEndOfTurn
	{
		void NextTurn();
	}
	public class EndOfTurn : IEndOfTurn
	{
		readonly IPlayersLoader players;
		readonly IProvincesLoader pr;
		readonly IActionLoader actions;
		readonly IActivePlayer active;
		readonly IPowersLoader powers;
		readonly INewGame newgame;
		public EndOfTurn(IPlayersLoader players, IProvincesLoader pr, IActionLoader actions, IActivePlayer active, IPowersLoader powers, INewGame newgame)
		{
			this.players = players;
			this.pr = pr;
			this.actions = actions;
			this.active = active;
			this.powers = powers;
			this.newgame = newgame;
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
		public void NextTurn()
		{
			End();
			AllRobotsActions();
			players.Save();
			pr.Save();
			actions.Save();
			if (LivingHumans < 1 || powers.Last.MajorityReached)
			{
				newgame.Finish();
			}
		}
	}
}
