using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Bishop")]
    public class Bishop : PieceBase
    {
        public Bishop(ChessColor color) : base(color)
        {
        }
    }
}
