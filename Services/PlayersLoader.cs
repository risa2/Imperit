using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Imperit.State;

namespace Imperit.Services
{
    public interface IPlayersLoader : IReadOnlyList<Player>
    {
        void Save();
        void Clear();
        void Add(Player player);
        void Set(IReadOnlyList<Player> new_players);
    }
    public class PlayersLoader : IPlayersLoader
    {
        readonly Load.JsonWriter<Load.JsonPlayer, Player, Settings> loader;
        readonly List<State.Player> players;
        public PlayersLoader(IServiceIO io, ISettingsLoader sl)
        {
            loader = new Load.JsonWriter<Load.JsonPlayer, Player, Settings>(io.Players, sl.Settings, Load.JsonPlayer.From);
            players = loader.Load().ToList();
        }
        public int Count => players.Count;
        public bool IsReadOnly => ((ICollection<Player>)players).IsReadOnly;
        public Player this[int i] => players[i];
        public void Save() => loader.Save(players);
        public void Add(Player player)
        {
            players.Add(player);
            loader.Add(player);
        }
        public void Clear()
        {
            players.Clear();
            loader.Clear();
        }
        public IEnumerator<Player> GetEnumerator() => players.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => players.GetEnumerator();

        public void Set(IReadOnlyList<Player> new_players)
        {
            for (int i = 0; i < players.Count && i < new_players.Count; ++i)
            {
                players[i] = new_players[i];
            }
        }
    }
}