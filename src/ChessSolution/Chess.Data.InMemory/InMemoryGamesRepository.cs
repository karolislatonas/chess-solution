using Chess.Domain;
using System.Collections.Concurrent;
using System.Linq;

namespace Chess.Data.InMemory
{
    public class InMemoryGamesRepository : IGamesRepository
    {
        private readonly ConcurrentBag<Game> games;

        public InMemoryGamesRepository()
        {
            games = new ConcurrentBag<Game>();
        }

        public void AddGame(Game game)
        {
            games.Add(game);
        }

        public Game GetGame(string gameId)
        {
            return games.Single(g => g.GameId == gameId);
        }
    }
}
