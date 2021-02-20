using Chess.Domain.Movement;
using System.Collections.Concurrent;
using System.Linq;

namespace Chess.Data.InMemory
{
    public class InMemoryMovesRepository : IMovesRepository
    {
        private readonly ConcurrentBag<(string gameId, PieceMove move)> moves;

        public InMemoryMovesRepository()
        {
            moves = new ConcurrentBag<(string gameId, PieceMove move)>();
        }

        public void AddMove(string gameId, PieceMove pieceMove)
        {
            moves.Add((gameId, pieceMove));
        }

        public PieceMove[] GetGameMoves(string gameId)
        {
            return moves
                .Where(m => m.gameId == gameId)
                .Select(m => m.move)
                .OrderBy(m => m.SequenceNumber)
                .ToArray();
        }
    }
}
