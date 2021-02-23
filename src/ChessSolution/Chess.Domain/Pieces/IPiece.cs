using Chess.Domain.Movement.Movers;

namespace Chess.Domain.Pieces
{
    public interface IPiece
    {
        ChessColor Color { get; }

        IMover Mover { get; }
    }
}
