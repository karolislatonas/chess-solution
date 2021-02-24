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

            return GetAvailableMoves(pawn, board, from)
                .Concat(GetAvailableTakes(pawn, board, from));
        }

        private static IEnumerable<Location> GetAvailableMoves(Pawn pawn, Board board, Location from)
        {
            var availableMoves = new List<Location>(2);

            availableMoves.Add(from.AddRows(pawn.RowMoveDirection));

            if (from.Row == pawn.StartingRow)
                availableMoves.Add(from.AddRows(2 * pawn.RowMoveDirection));

            return availableMoves
                .Where(l => !board.ContainsPieceAt(l))
                .Where(board.IsWithinBoard);
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
                .Where(board.IsWithinBoard)
                .Where(board.ContainsPieceAt)
                .Where(l => AreOppositeColors(pawn, board.GetPieceAt(l)));
        }

        private static bool AreOppositeColors(IPiece piece, IPiece other)
        {
            return piece.Color != other.Color;
        }
    }
}
