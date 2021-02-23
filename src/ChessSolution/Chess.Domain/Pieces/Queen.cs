using Chess.Domain.Movement.Movers;
using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Queen")]
    public class Queen : PieceBase
    {
        public Queen(ChessColor color) : base(color)
        {
        }

        public override IMover Mover => new QueenMover();
    }
}
