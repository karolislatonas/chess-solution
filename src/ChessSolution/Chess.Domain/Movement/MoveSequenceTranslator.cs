using Chess.Domain.Movement.Moves;
using System;
using System.Linq;

namespace Chess.Domain.Movement
{
    public class MoveSequenceTranslator
    {
        private readonly MovesReplayer movesReplayer;

        public MoveSequenceTranslator()
        {
            movesReplayer = new MovesReplayer(new MovesLog());
        }

        public IMove TranslateNextMove(PieceMove pieceMove)
        {
            EnsureNextMoveIsInSequence(pieceMove);

            return TranslateNextMove(pieceMove.From, pieceMove.To);
        }

        public IMove TranslateNextMove(Location from, Location to)
        {
            var piece = movesReplayer.Board.GetPieceAt(from);

            var move = piece.Mover
                .GetAvailableMovesFrom(movesReplayer.Board, movesReplayer.MovesLog, from)
                .First(m => m.To == to);

            movesReplayer.AddMove(move);
            movesReplayer.ToLastMove();

            return move;
        }

        private void EnsureNextMoveIsInSequence(PieceMove pieceMove)
        {
            var nextMoveSequenceNumber = movesReplayer.MovesLog.NextMoveSequenceNumber();

            if (pieceMove.SequenceNumber != nextMoveSequenceNumber)
                throw new Exception();
        }
    }
}
