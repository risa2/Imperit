using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Imperit.Load;
using Imperit.State;

namespace Imperit.Services
{
	public interface IFormerPlayersLoader : IReadOnlyList<Player>
	{
		void Reset(IEnumerable<Player> players);
	}
	public class FormerPlayersLoader : IFormerPlayersLoader
	{
		readonly JsonWriter<JsonPlayer, Player, Settings> loader;
		ImmutableArray<Player> players;
		public FormerPlayersLoader(IServiceIO io, ISettingsLoader sl)
		{
			loader = new JsonWriter<JsonPlayer, Player, Settings>(io.Players, sl.Settings, JsonPlayer.From);
			players = loader.Load().ToImmutableArray();
		}
		public int Count => players.Length;
		public Player this[int i] => players[i];
		public IEnumerator<Player> GetEnumerator() => (players as IEnumerable<Player>).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public void Reset(IEnumerable<Player> new_players) => players = new_players.ToImmutableArray();
	}
}