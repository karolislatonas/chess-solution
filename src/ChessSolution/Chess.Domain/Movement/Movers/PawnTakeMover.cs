using Chess.Domain.Extensions;
using Chess.Domain.Movement.Moves;
using Chess.Domain.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class PawnTakeMover : IMover
    {
        public IEnumerable<IMove> GetAvailableMovesFrom(Board board, Location from)
        {
            var piece = board.GetPieceAt<Pawn>(from);

            return piece
                 .TakeDirections
                 .Select(d => from.Add(d))
                 .Where(l => board.IsPieceOfColor(l, piece.Color.GetOpposite()))
                 .Select(l => new TakeMove(from, l));
        }
    }
}
