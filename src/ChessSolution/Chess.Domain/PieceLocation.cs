using Chess.Domain.Movement;
using Chess.Domain.Pieces;

namespace Chess.Domain
{
    public class PieceLocation
    {
        public PieceLocation(IPiece piece, Location location)
        {
            Piece = piece;
            Location = location;
        }

        public IPiece Piece { get; }

        public Location Location { get; }
    }
}
