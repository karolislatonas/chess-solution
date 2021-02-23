using Chess.Domain.Movement.Movers;
using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Rook")]
    public class Rook : PieceBase
    {
        public Rook(ChessColor color) : base(color)
        {
        }

        public override IMover Mover => new RookMover();
    }
}
