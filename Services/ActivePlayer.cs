using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IActivePlayer
    {
        int Id { get; }
        void Next(IReadOnlyList<State.Player> players);
        void Reset();
    }
    public class ActivePlayer : IActivePlayer
    {
        readonly Load.IFile inout;
        public ActivePlayer(IServiceIO io) => inout = io.Active;
        public int Id => int.Parse(inout.Read());
        void SetIndex(int i) => inout.Write(i.ToString());
        public void Next(IReadOnlyList<State.Player> players)
        {
            int id = Id + 1;
            int next = Enumerable.Range(0, players.Count).Select(i => (i + id) % players.Count).FirstOr(i => players[i].Alive, -1);
            SetIndex(next == -1 ? Id : next);
        }
        public void Reset() => SetIndex(0);
    }
}