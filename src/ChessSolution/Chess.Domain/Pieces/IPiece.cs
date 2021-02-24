using Chess.Domain.Movement;
using Chess.Domain.Movement.Movers;
using System.Collections.Generic;

namespace Chess.Domain.Pieces
{
    public interface IPiece
    {
        ChessColor Color { get; }

        IMover Mover { get; }

        IReadOnlyList<Location> MoveDirections { get; }

        IReadOnlyList<Location> TakeDirections { get; }
    }
}
