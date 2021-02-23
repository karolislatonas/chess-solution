using Chess.Domain.Extensions;
using Chess.Domain.Movement.Movers;
using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Pawn")]
    public class Pawn : PieceBase
    {
        public Pawn(ChessColor color) : base(color)
        {

        }

        public int StartingRow => Color.IsWhite() ? 2 : 7;

        public int RowMoveDirection => Color.IsWhite() ? 1 : -1;

        public override IMover Mover => new PawnMover();
    }
}
