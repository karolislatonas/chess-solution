using Chess.Domain.Movement;
using Chess.Domain.Movement.Movers;
using System.Collections.Generic;

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

        public abstract IReadOnlyList<Location> MoveDirections { get; }

        public abstract IReadOnlyList<Location> TakeDirections { get; }
    }
}
