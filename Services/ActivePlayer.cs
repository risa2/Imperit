using Imperit.State;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IActivePlayer
    {
        int Id { get; }
        void Next(IReadOnlyList<Player> players);
        void Reset(IReadOnlyList<Player> players);
    }
    public class ActivePlayer : IActivePlayer
    {
        readonly Load.IFile inout;
        public ActivePlayer(IServiceIO io) => inout = io.Active;
        public int Id
        {
            get => int.Parse(inout.Read(), ExtMethods.Culture);
            private set => inout.Write(value.ToString(ExtMethods.Culture));
        }
        public void Next(IReadOnlyList<Player> players)
        {
            int id = Id + 1;
            int next = Enumerable.Range(0, players.Count).Select(i => (i + id) % players.Count).FirstOr(i => players[i].Alive && !(players[i] is Savage), -1);
            Id = next == -1 ? Id : next;
        }
        public void Reset(IReadOnlyList<Player> players) => Id = players.First(p => !(p is Savage)).Id;
    }
}