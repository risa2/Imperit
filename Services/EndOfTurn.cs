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
		public EndOfTurn(IPlayersLoader players, IProvincesLoader pr, IActionLoader actions, IActivePlayer active, IPowersLoader powers)
		{
			this.players = players;
			this.pr = pr;
			this.actions = actions;
			this.active = active;
			this.powers = powers;
		}
		bool AreHumansAlive => players.Any(player => !(player is State.Robot) && player.Alive);
		void End()
		{
			actions.EndOfTurn(active.Id);
			powers.Add(players);
			active.Next(players);
		}
		void RobotThink(State.Robot robot) => _ = actions.Add(robot.Think(pr));
		void AllRobotsactions()
		{
			while (players[active.Id] is State.Robot robot && AreHumansAlive)
			{
				RobotThink(robot);
				End();
			}
		}
		void RobotThinkingIfRobotIsPlaying()
		{
			if (players[active.Id] is State.Robot robot)
			{
				RobotThink(robot);
			}
		}
		public void NextTurn()
		{
			RobotThinkingIfRobotIsPlaying();
			End();
			AllRobotsactions();
			players.Save();
			pr.Save();
			actions.Save();
		}
	}
}
