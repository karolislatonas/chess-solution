using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Knight")]
    public class Knight : PieceBase
    {
        public Knight(ChessColor color) : base(color)
        {
        }
    }
}
