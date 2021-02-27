using Chess.Shared.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement
{
    public class MovesLog : IEnumerable<PieceMove>
    {
        private const int SequenceNumberStart = 0;
        private const int SequenceNumberIncrement = 1;

        private readonly IndexedLinkedList<int, PieceMove> moves;

        public MovesLog() :
            this(Enumerable.Empty<PieceMove>())
        {
            
        }

        public MovesLog(IEnumerable<PieceMove> pieceMoves)
        {
            moves = new IndexedLinkedList<int, PieceMove>(p => p.SequenceNumber, pieceMoves);

            EnsureSequenceWithoutGaps();
        }

        public PieceMove LatestMove => moves.LastOrDefault();

        public void AddMove(PieceMove pieceMove)
        {
            if (NextMoveSequenceNumber() != pieceMove.SequenceNumber)
                throw new Exception();

            moves.Add(pieceMove);
        }

        public void AddMove(Location from, Location to)
        {
            moves.Add(CreateNextMove(from, to));
        }

        public IEnumerable<PieceMove> GetMovesFromTo(int fromSequenceNumber, int toSequenceNumber)
        {
            for(var i = fromSequenceNumber; i <= toSequenceNumber; i++)
            {
                yield return moves.GetValue(i);
            }
        }

        public IEnumerator<PieceMove> GetEnumerator()
        {
            return moves.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private PieceMove CreateNextMove(Location from, Location to)
        {
            return new PieceMove(NextMoveSequenceNumber(), from, to);
        }

        private int NextMoveSequenceNumber()
        {
            return GetLatestMoveSequenceNumber() + SequenceNumberIncrement;
        }

        private int GetLatestMoveSequenceNumber()
        {
            var isLogEmpty = !moves.Any();
            if (isLogEmpty)
                return SequenceNumberIncrement;

            return moves.Last.SequenceNumber;
        }

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
