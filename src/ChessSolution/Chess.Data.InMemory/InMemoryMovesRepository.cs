using Chess.Data.Movement;
using System.Collections.Concurrent;
using System.Linq;

namespace Chess.Data.InMemory
{
    public class InMemoryMovesRepository : IMovesRepository
    {
        private readonly ConcurrentBag<PieceMoveDto> moves;

        public InMemoryMovesRepository()
        {
            moves = new ConcurrentBag<PieceMoveDto>();
        }

        public void AddMove(PieceMoveDto pieceMove)
        {
            moves.Add(pieceMove);
        }

        public PieceMoveDto[] GetGameMoves(int gameId)
        {
            return moves
                .Where(m => m.GameId == gameId)
                .OrderBy(m => m.SequenceNumber)
                .ToArray();
        }
    }
}
