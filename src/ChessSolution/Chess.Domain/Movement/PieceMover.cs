using Chess.Domain.Movement.Movers;
using Chess.Domain.Movement.Moves;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement
{
    public class PieceMover
    {
        public void EnsureIsValidMove(Board board, Location from, Location to)
        {
            var mover = GetPieceMover(board, from);

            var canMoveTo = mover.GetAvailableMovesFrom(board, from).Any(l => l == to);

            if (!canMoveTo)
            {
                throw new Exception("Invalid move");
            }
        }

        public HashSet<Location> GetAvailableMoves(Board board, Location from)
        {
            var mover = GetPieceMover(board, from);

            return new HashSet<Location>(mover.GetAvailableMovesFrom(board, from));
        }

        public IMove CreateMove(Board board, Location from, Location to)
        {
            return GetPieceMover(board, from).CreateMove(board, from, to);
        }

        private IMover GetPieceMover(Board board, Location from)
        {
            var piece = board.GetPieceAt(from);

            return piece.Mover;
        }
    }
}
