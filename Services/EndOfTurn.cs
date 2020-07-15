using System.Linq;

namespace Imperit.Services
{
    public interface IEndOfTurn
    {
        void Next();
    }
    public class EndOfTurn : IEndOfTurn
    {
        readonly ISettingsLoader sl;
        readonly IPlayersLoader players;
        readonly IProvincesLoader pr;
        readonly IActionLoader Actions;
        readonly IActivePlayer active;
        readonly IPowersLoader powers;
        public EndOfTurn(ISettingsLoader sl, IPlayersLoader players, IProvincesLoader pr, IActionLoader Actions, IActivePlayer active, IPowersLoader powers)
        {
            this.sl = sl;
            this.players = players;
            this.pr = pr;
            this.Actions = Actions;
            this.active = active;
            this.powers = powers;
        }
        bool AreHumansAlive => players.Any(player => !(player is State.Robot) && player.Alive);
        void End()
        {
            Actions.EndOfTurn(active.Id);
            powers.Add(players);
            active.Next(players);
        }
        void RobotThink(State.Robot robot) => _ = Actions.Add(robot.Think(sl.Settings, pr.Provinces));
        void AllRobotsActions()
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
        public void Next()
        {
            RobotThinkingIfRobotIsPlaying();
            End();
            AllRobotsActions();
            players.Save();
            pr.Save();
            Actions.Save();
        }
    }
}
