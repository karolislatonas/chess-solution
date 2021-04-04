using Chess.Domain.Movement.Moves;
using System.Collections.Generic;

namespace Chess.Domain.Movement
{
    public class MovesReplayer
    {
        private MovesReplayer(MovesLog movesLog)
        {   
            MovesLog = movesLog;
            Board = new Board();
        }

        public Board Board { get; private set; }

        public MovesLog MovesLog { get; }

        public bool IsAtLastMove => (MovesLog.LastMove?.SequenceNumber ?? 0) == CurrentMoveNumber;

        public int CurrentMoveNumber { get; private set; }

        public IMove GetCurrentMove()
        {
            if (MovesLog.TryGetMove(CurrentMoveNumber, out var moveItem))
                return moveItem.Move;

            return null;
        }

        public void AddMoves(IEnumerable<IMove> moves)
        {
            MovesLog.AddMoves(moves);
        }

        public void AddMoveAndReplay(IMove move)
        {
            AddMove(move);
            ToLastMove();
        }

        public void AddMove(IMove move)
        {
            MovesLog.AddMove(move);
        }

        public void ToPreviousMove()
        {
            ToMove(PreviousMoveNumber());
        }

        public void ToNextMove()
        {
            ToMove(NextMoveNumber());
        }

        public void ToStart()
        {
            ToMove(0);
        }

        public void ToLastMove()
        {
            var lastMove = MovesLog.LastMove;

            if(lastMove != null)
                ToMove(lastMove.SequenceNumber);
        }

        public void ToMove(int sequenceNumber)
        {
            if (sequenceNumber > CurrentMoveNumber)
                ApplyMoves(NextMoveNumber(), sequenceNumber);

            if (sequenceNumber < CurrentMoveNumber)
            {
                Reset();
                ApplyMoves(1, sequenceNumber);
            }
        }

        public static MovesReplayer CreateAndReplay(MovesLog movesLog)
        {
            var movesReplayer = new MovesReplayer(movesLog);
            movesReplayer.ToLastMove();

            return movesReplayer;
        }

        public static MovesReplayer Create(MovesLog movesLog)
        {
            return new MovesReplayer(movesLog);
        }

        public static MovesReplayer Create()
        {
            return Create(new MovesLog());
        }

        private int PreviousMoveNumber() => CurrentMoveNumber - 1;

        private int NextMoveNumber() => CurrentMoveNumber + 1;

        public void Reset()
        {
            CurrentMoveNumber = 0;
            Board = new Board();
        }

        private void ApplyMoves(int fromSequenceNumber, int toSequenceNumber)
        {
            var movesItem = MovesLog.GetMovesFromTo(fromSequenceNumber, toSequenceNumber);

            foreach(var moveItem in movesItem)
            {
                CurrentMoveNumber = moveItem.SequenceNumber;
                Board.ApplyMove(moveItem.Move);
            }
        }
    }
}
