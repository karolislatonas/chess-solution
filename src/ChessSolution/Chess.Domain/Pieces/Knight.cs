using Chess.Domain.Movement.Movers;
using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Knight")]
    public class Knight : PieceBase
    {
        public Knight(ChessColor color) : base(color)
        {
        }

        public override IMover Mover => new NullMover();
    }
}
