using System.Linq;

namespace Imperit.Services
{
    public interface IEndOfTurnManager
    {
        void Next();
    }
    public class EndOfTurnManager : IEndOfTurnManager
    {
        readonly ISettingsLoader sl;
        readonly IPlayersLoader players;
        readonly IProvincesLoader pr;
        readonly IActionWriter actions;
        readonly IActivePlayer active;
        readonly IPlayersPowers powers;
        public EndOfTurnManager(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader pr, IActionWriter actions, IActivePlayer active, IPlayersPowers powers)
        {
            this.sl = sl;
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
        void AllRobotsActions()
        {
            while (players[active.Id] is State.Robot robot && AreHumansAlive)
            {
                actions.Add(robot.Think(sl.Settings, pr.Provinces), save: false);
                End();
            }
        }
        void RobotThinkingIfRobotIsPlaying()
        {
            if (players[active.Id] is State.Robot robot)
            {
                actions.Add(robot.Think(sl.Settings, pr.Provinces), save: false);
            }
        }
        public void Next()
        {
            RobotThinkingIfRobotIsPlaying();
            End();
            AllRobotsActions();
            players.Save();
            pr.Save();
            actions.Save();
        }
    }
}
