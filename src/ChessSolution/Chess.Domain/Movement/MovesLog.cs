using Chess.Domain.Movement.Moves;
using Chess.Shared.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement
{
    public class MovesLog : IEnumerable<MoveSequenceItem>
    {
        private const int SequenceNumberStart = 0;
        private const int SequenceNumberIncrement = 1;

        private readonly IndexedLinkedList<int, MoveSequenceItem> movesSequence;

        public MovesLog() :
            this(Enumerable.Empty<IMove>())
        {
            
        }

        public MovesLog(IEnumerable<IMove> moves)
        {
            movesSequence = new IndexedLinkedList<int, MoveSequenceItem>(p => p.SequenceNumber);

            AddMoves(moves);
        }

        public MoveSequenceItem LastMove => movesSequence.LastOrDefault();

        public void AddMoves(IEnumerable<IMove> moves)
        {
            foreach (var move in moves)
                AddMove(move);
        }

        public void AddMove(IMove move)
        {
            var newItem = new MoveSequenceItem(NextMoveSequenceNumber(), move);
            
            movesSequence.Add(newItem);
        }

        public bool TryGetMove(int sequenceNumber, out MoveSequenceItem move)
        {
            return movesSequence.TryGetValue(sequenceNumber, out move);
        }

        public IEnumerable<MoveSequenceItem> GetMovesFromTo(int fromSequenceNumber, int toSequenceNumber)
        {
            var fromNumber = Math.Max(fromSequenceNumber, 1);
            var toNumber = Math.Min(toSequenceNumber, LastMove?.SequenceNumber ?? 0);

            for (var i = fromNumber; i <= toNumber; i++)
            {
                yield return movesSequence.GetValue(i);
            }
        }

        public int NextMoveSequenceNumber()
        {
            return GetLatestMoveSequenceNumber() + SequenceNumberIncrement;
        }

        public IEnumerator<MoveSequenceItem> GetEnumerator()
        {
            return movesSequence.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int GetLatestMoveSequenceNumber()
        {
            var isLogEmpty = !movesSequence.Any();
            if (isLogEmpty)
                return SequenceNumberStart;

            return movesSequence.Last.SequenceNumber;
        }

        private void EnsureAreEqual(int num1, int num2, Action throwAction)
        {
            if (num1 != num2)
                throwAction();

        }

      
    }
}
