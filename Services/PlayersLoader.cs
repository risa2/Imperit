using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imperit.Services
{
    public interface IPlayersLoader : IArray<State.Player>
    {
        void Save();
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
        public State.Player this[int i]
        {
            get => players[i];
            set => players[i] = value;
        }
        public void Save() => loader.Save(players);
        public void Add(State.Player player)
        {
            players.Add(player);
            loader.Add(player);
        }
        public void Clear()
        {
            players.Clear();
            loader.Save(new State.Player[0]);
        }
        public IEnumerator<State.Player> GetEnumerator() => players.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => players.GetEnumerator();

        public int IndexOf(State.Player item) => players.IndexOf(item);
        public void Insert(int index, State.Player item) => throw new System.NotSupportedException("Players may be inserted only at end.");
        public void RemoveAt(int index) => throw new System.NotSupportedException("Players may not be removed.");
        public bool Contains(State.Player item) => players.Contains(item);
        public void CopyTo(State.Player[] array, int arrayIndex) => players.CopyTo(array, arrayIndex);
        public bool Remove(State.Player item) => throw new System.NotSupportedException("Players may not be removed.");
    }
}