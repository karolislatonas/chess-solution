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
                .TakeWhile(l => !board.ContainsPieceAt(l));
        }

        private static IEnumerable<Location> GetPossibleMoveDirections(Pawn pawn, Location from)
        {
            var moveDirections = pawn.MoveDirections.AsEnumerable();

            if (from.Row == pawn.StartingRow)
                moveDirections = moveDirections.Append(new Location(0, 2 * pawn.RowMoveDirection));

            return moveDirections;
        }

      
    }
}
