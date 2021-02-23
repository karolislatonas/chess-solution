using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("King")]
    public class King : PieceBase
    {
        public King(ChessColor color) : base(color)
        {
        }
    }
}
