using Chess.Domain.Movement.Movers;

namespace Chess.Domain.Pieces
{
    public abstract class PieceBase : IPiece
    {
        protected PieceBase(ChessColor color)
        {
            Color = color;
        }

        public ChessColor Color { get; }

        public abstract IMover Mover { get; }
    }
}
