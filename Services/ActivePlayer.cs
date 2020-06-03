using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IActivePlayer
    {
        int Id { get; }
        void Next(IReadOnlyList<State.Player> players);
        void StartGame();
    }
    public class ActivePlayer : IActivePlayer
    {
        Load.IFile inout;
        public int Id { get; private set; }
        public ActivePlayer(IServiceIO io)
        {
            inout = io.Active;
            Id = int.Parse(inout.Read());
        }
        public void Next(IReadOnlyList<State.Player> players)
        {
            int next = Enumerable.Range(0, players.Count).Select(i => (i + Id + 1) % players.Count).FirstOr(i => players[i].Alive, -1);
            SetIndex(next == -1 ? Id : next);
        }
        public void StartGame() => SetIndex(0);
        private void SetIndex(int i)
        {
            Id = i;
            inout.Write(i.ToString());
        }
    }
}