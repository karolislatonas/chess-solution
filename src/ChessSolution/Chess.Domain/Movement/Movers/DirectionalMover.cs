using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public abstract class DirectionalMover : PieceMoverBase
    {
        public override IEnumerable<Location> GetAvaialbleMovesFrom(Board board, Location from)
        {
            return GetPossibleMoveDirections()
                .SelectMany(d => GetAvailablesMovesInDirection(board, from, d))
                .Where(board.IsWithinBoard);
        }

        private static IEnumerable<Location> GetAvailablesMovesInDirection(Board board, Location from, Location direction)
        {
            var bishop = board.GetPieceAt(from);
            var location = from.Add(direction);

            var isBlockedByOtherPiece = false;

            while (board.IsWithinBoard(location) &&
                   !board.IsPieceOfColor(location, bishop.Color) &&
                   !isBlockedByOtherPiece)
            {
                yield return location;

                isBlockedByOtherPiece = board.ContainsPieceAt(location);
                location = location.Add(direction);
            }
        }

        protected abstract IEnumerable<Location> GetPossibleMoveDirections();
    }
}
