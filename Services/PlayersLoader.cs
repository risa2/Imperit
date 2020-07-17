using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IPlayersLoader : IReadOnlyList<State.Player>
    {
        void Save();
        void Clear();
        void Add(State.Player player);
        void Set(IReadOnlyList<State.Player> new_players);
    }
    public class PlayersLoader : IPlayersLoader
    {
        readonly Load.Writer<Load.Player, State.Player, bool> loader;
        readonly List<State.Player> players;
        public PlayersLoader(IServiceIO io)
        {
            loader = new Load.Writer<Load.Player, State.Player, bool>(io.Players, false, Load.Player.FromPlayer);
            players = loader.Load().ToList();
        }
        public int Count => players.Count;
        public bool IsReadOnly => ((ICollection<State.Player>)players).IsReadOnly;
        public State.Player this[int i] => players[i];
        public void Save() => loader.Save(players);
        public void Add(State.Player player)
        {
            players.Add(player);
            loader.Add(player);
        }
        public void Clear()
        {
            players.Clear();
            loader.Clear();
        }
        public IEnumerator<State.Player> GetEnumerator() => players.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => players.GetEnumerator();

        public void Set(IReadOnlyList<State.Player> new_players)
        {
            for (int i = 0; i < players.Count && i < new_players.Count; ++i)
            {
                players[i] = new_players[i];
            }
        }
    }
}