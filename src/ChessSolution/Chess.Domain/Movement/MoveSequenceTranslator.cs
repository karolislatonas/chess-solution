using Chess.Domain.Movement.Moves;
using System;

namespace Chess.Domain.Movement
{
    public class MoveSequenceTranslator
    {
        private readonly PieceMover pieceMover;
        private readonly MovesReplayer movesReplayer;

        public MoveSequenceTranslator()
        {
            movesReplayer = MovesReplayer.Create();
            pieceMover = new PieceMover();
        }

        public IMove TranslateNextMove(PieceMove pieceMove)
        {
            EnsureNextMoveIsInSequence(pieceMove);

            return TranslateNextMove(pieceMove.From, pieceMove.To);
        }

        public IMove TranslateNextMove(Location from, Location to)
        {
            var move = pieceMover.GetMove(movesReplayer.Board, movesReplayer.MovesLog, from, to);

            movesReplayer.AddMoveAndReplay(move);

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
