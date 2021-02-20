using Chess.Domain;
using System.Collections.Concurrent;

namespace Chess.Data.InMemory
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly ConcurrentBag<Game> games;

        public InMemoryGameRepository()
        {
            games = new ConcurrentBag<Game>();
        }

        public void AddGame(Game game)
        {
            games.Add(game);
        }
    }
}
