using Chess.Domain.Extensions;
using Chess.Domain.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class PawnMover : PieceMoverBase
    {
        public override IEnumerable<Location> GetAvailableMovesFrom(Board board, Location from)
        {
            var pawn = board.GetPieceAt<Pawn>(from);

            return GetPossibleMoveDirections(pawn, from)
                .Select(d => from.Add(d))
                .TakeWhile(l => !board.ContainsPieceAt(l))

                .Concat(GetAvailableTakes(pawn, board, from));
        }

        private static IEnumerable<Location> GetPossibleMoveDirections(Pawn pawn, Location from)
        {
            yield return new Location(0, pawn.RowMoveDirection);

            if (from.Row == pawn.StartingRow)
                yield return new Location(0, 2 * pawn.RowMoveDirection);
        }

        private static IEnumerable<Location> GetAvailableTakes(Pawn pawn, Board board, Location from)
        {
            var possibleTakeDirections = new[]
            {
                new Location(-1, pawn.RowMoveDirection),
                new Location(1, pawn.RowMoveDirection),
            };

            return possibleTakeDirections
                .Select(d => from.Add(d))
                .Where(l => board.IsPieceOfColor(l, pawn.Color.GetOpposite()));
        }
    }
}
