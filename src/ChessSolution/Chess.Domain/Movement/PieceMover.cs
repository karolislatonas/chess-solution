using Chess.Domain.Movement.Movers;
using System;
using System.Collections.Generic;

namespace Chess.Domain.Movement
{
    public class PieceMover
    {
        private readonly Board board;

        public PieceMover(Board board)
        {
            this.board = board;
        }

        public void EnsureIsValidMove(Location from, Location to)
        {
            var mover = GetPieceMover(from);

            var canMoveTo = mover.CanMoveTo(board, from, to);

            if (!canMoveTo)
            {
                throw new Exception();
            }
        }

        public HashSet<Location> GetAvailableMoves(Location from)
        {
            var mover = GetPieceMover(from);

            return new HashSet<Location>(mover.GetAvaialbleMovesFrom(board, from));
        }

        private IMover GetPieceMover(Location from)
        {
            var piece = board.GetPieceAt(from);

            return piece.Mover;
        }
    }
}
