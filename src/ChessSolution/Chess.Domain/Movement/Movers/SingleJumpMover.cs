using Chess.Domain.Movement.Moves;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class SingleJumpMover : IMover
    {
        public IEnumerable<IMove> GetAvailableMovesFrom(Board board, Location from)
        {
            var piece = board.GetPieceAt(from);

            return piece
                .MoveDirections
                .Select(d => from.Add(d))
                .Where(board.IsWithinBoard)
                .Where(l => !board.IsPieceOfColor(l, piece.Color))
                .Select(l => CreateMove(board, from, l));
        }

        public IMove CreateMove(Board board, Location from, Location to)
        {
            if (board.ContainsPieceAt(to))
                return new TakeMove(from, to);

            return new SimpleMove(from, to);
        }
    }
}
