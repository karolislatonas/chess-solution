using Chess.Domain;
using System.Collections.Concurrent;

namespace Chess.Data.InMemory
{
    public class InMemoryGamesRepository : IGamesRepository
    {
        private readonly ConcurrentDictionary<string, Game> games;

        public InMemoryGamesRepository()
        {
            games = new ConcurrentDictionary<string, Game>();
        }

        public void AddGame(Game game)
        {
            games.TryAdd(game.GameId, game);
        }

        public Game GetGame(string gameId)
        {
            return games[gameId];
        }

        public void UpdateGame(Game game)
        {
            games.AddOrUpdate(game.GameId, game, (id, g) => game);
        }
    }
}
