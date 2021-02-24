using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class SingleJumpMover : PieceMoverBase
    {
        public override IEnumerable<Location> GetAvailableMovesFrom(Board board, Location from)
        {
            var piece = board.GetPieceAt(from);

            return piece
                .MoveDirections
                .Select(d => from.Add(d))
                .Where(board.IsWithinBoard)
                .Where(l => !board.IsPieceOfColor(l, piece.Color));
        }
    }
}
