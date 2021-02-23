using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Pawn")]
    public class Pawn : PieceBase
    {
        public Pawn(ChessColor color) : base(color)
        {
        }
    }
}
