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
        public int Id
        {
            get => int.Parse(inout.Read());
            private set => inout.Write(value.ToString());
        }
        public void Next(IReadOnlyList<State.Player> players)
        {
            int id = Id + 1;
            int next = Enumerable.Range(0, players.Count).Select(i => (i + id) % players.Count).FirstOr(i => players[i].Alive, -1);
            Id = next == -1 ? Id : next;
        }
        public void Reset() => Id = 0;
    }
}