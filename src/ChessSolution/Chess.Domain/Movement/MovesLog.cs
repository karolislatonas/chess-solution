using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement
{
    public class MovesLog
    {
        private const int SequenceNumberStart = 0;
        private const int SequenceNumberIncrement = 1;

        private readonly List<PieceMove> moves;

        public MovesLog() :
            this(Enumerable.Empty<PieceMove>())
        {
            
        }

        public PieceMove LatestMove => moves.LastOrDefault();

        public MovesLog(IEnumerable<PieceMove> pieceMoves)
        {
            moves = new List<PieceMove>(pieceMoves);

            EnsureSequenceWithoutGaps();
        }

        public void AddMove(Location from, Location to)
        {
            moves.Add(GetNextMove(from, to));
        }

        public PieceMove GetNextMove(Location from, Location to)
        {
            var nextMoveSequenceNumber = GetLatestMoveSequenceNumber() + SequenceNumberIncrement;

            return new PieceMove(nextMoveSequenceNumber, from, to);
        }

        private int GetLatestMoveSequenceNumber()
        {
            var isLogEmpty = !moves.Any();
            if (isLogEmpty)
                return SequenceNumberIncrement;

            return moves[moves.Count - 1].SequenceNumber;
        }

        public IReadOnlyList<PieceMove> Moves => moves;

        private void EnsureSequenceWithoutGaps()
        {
            var expectedSequenceNumber = SequenceNumberStart + SequenceNumberIncrement;

            foreach (var move in moves)
            {
                EnsureAreEqual(move.SequenceNumber, expectedSequenceNumber, () => throw new Exception("Invalid sequence number"));
                expectedSequenceNumber += SequenceNumberIncrement;
            }
        }


        private void EnsureAreEqual(int num1, int num2, Action throwAction)
        {
            if (num1 != num2)
                throwAction();

        }
    }
}
