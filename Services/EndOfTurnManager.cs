using System.Linq;

namespace Imperit.Services
{
    public interface IEndOfTurnManager
    {
        int Next();
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
        bool IsRobotPlaying => !players[active.Id].IsHuman;
        bool AreHumansAlive => players.Any(player => player.IsHuman && player.Alive);
        void End()
        {
            actions.EndOfTurn(active.Id);
            powers.Add(players);
            active.Next(players);
        }
        void RobotAction()
        {
            actions.Add(new Dynamics.Brain(players[active.Id]).Think(sl.Settings, pr.Provinces));
        }
        void AllRobotsActions()
        {
            while (IsRobotPlaying && AreHumansAlive)
            {
                RobotAction();
                End();
            }
        }
        void RobotThinkingIfRobotIsPlaying()
        {
            if (IsRobotPlaying)
            {
                RobotAction();
            }
        }
        public int Next()
        {
            RobotThinkingIfRobotIsPlaying();
            End();
            AllRobotsActions();
            players.Save();
            pr.Save();
            return active.Id;
        }
    }
}
