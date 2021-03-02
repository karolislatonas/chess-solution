using Chess.Domain.Movement.Moves;
using Chess.Domain.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class PawnMover : IMover
    {
        public IEnumerable<IMove> GetAvailableMovesFrom(Board board, Location from)
        {
            var pawn = board.GetPieceAt<Pawn>(from);

            return GetPossibleMoveDirections(pawn, from)
                .Select(d => from.Add(d))
                .TakeWhile(l => !board.ContainsPieceAt(l))
                .Select(l => new SimpleMove(from, l));
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
