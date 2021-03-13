﻿using Chess.Domain.Movement.Moves;

namespace Chess.Domain.Movement
{
    public class MovesReplayer
    {
        public MovesReplayer(MovesLog movesLog, int moveNumber = 0)
        {   
            MovesLog = movesLog;

            Reset();

            ToMove(moveNumber);
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
