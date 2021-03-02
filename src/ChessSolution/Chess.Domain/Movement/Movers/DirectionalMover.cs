using Chess.Domain.Movement.Moves;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class DirectionalMover : IMover
    {
        public IEnumerable<IMove> GetAvailableMovesFrom(Board board, MovesLog movesLog, Location from)
        {
            var possibleMoveDirections = board.GetPieceAt(from).MoveDirections;

            return possibleMoveDirections
                .SelectMany(d => GetAvailablesMovesInDirection(board, from, d))
                .Where(board.IsWithinBoard)
                .Select(l => CreateMove(board, from, l));
        }

        private static IEnumerable<Location> GetAvailablesMovesInDirection(Board board, Location from, Location direction)
        {
            var piece = board.GetPieceAt(from);

            var location = from.Add(direction);

            var isBlockedByOtherPiece = false;

            while (board.IsWithinBoard(location) &&
                   !board.IsPieceOfColor(location, piece.Color) &&
                   !isBlockedByOtherPiece)
            {
                yield return location;

                isBlockedByOtherPiece = board.ContainsPieceAt(location);
                location = location.Add(direction);
            }
        }

        public IMove CreateMove(Board board, Location from, Location to)
        {
            if (board.ContainsPieceAt(to))
                return new TakeMove(from, to);

            return new SimpleMove(from, to);
        }
    }
}
