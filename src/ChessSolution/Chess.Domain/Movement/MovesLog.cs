using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement
{
    public class MovesLog
    {
        private readonly List<PieceMove> moves;

        public MovesLog() :
            this(Enumerable.Empty<PieceMove>())
        {
            
        }

        public MovesLog(IEnumerable<PieceMove> pieceMoves)
        {
            moves = new List<PieceMove>(pieceMoves);
        }

        public void AddMove(PieceMove pieceMove)
        {
            moves.Add(pieceMove);
        }

        public PieceMove GetNextMove(Location from, Location to)
        {
            var nextMoveSequenceNumber = GetLatestMoveSequenceNumber() + 1;

            return new PieceMove(nextMoveSequenceNumber, from, to);
        }

        private int GetLatestMoveSequenceNumber()
        {
            var isLogEmpty = !moves.Any();
            if (isLogEmpty)
                return 0;

            return moves[moves.Count - 1].SequenceNumber;
        }

        public IReadOnlyList<PieceMove> Moves => moves;
    }
}
