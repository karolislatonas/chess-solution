using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class KingMover : PieceMoverBase
    {
        public override IEnumerable<Location> GetAvailableMovesFrom(Board board, Location from)
        {
            var piece = board.GetPieceAt(from);

            return GetPossibleMoveDirections()
                .Select(d => from.Add(d))
                .Where(board.IsWithinBoard)
                .Where(l => !board.IsPieceOfColor(l, piece.Color));

        }

        protected IEnumerable<Location> GetPossibleMoveDirections()
        {
            yield return new Location(1, 1);
            yield return new Location(-1, -1);
            yield return new Location(-1, 1);
            yield return new Location(1, -1);
            yield return new Location(1, 0);
            yield return new Location(0, 1);
            yield return new Location(-1, 0);
            yield return new Location(0, -1);
        }
    }
}
