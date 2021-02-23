using Chess.Domain.Movement;
using Chess.Domain.Pieces;
using System.Collections.Generic;

namespace Chess.WebUI.ViewModels
{
    public class PieceSelection
    {
        private readonly IReadOnlySet<Location> availableMoves;

        public PieceSelection(Location from, IPiece piece, IReadOnlySet<Location> availableMoves)
        {
            this.availableMoves = availableMoves;

            From = from;
            Piece = piece;
        }

        public Location From { get; }

        public IPiece Piece { get; }

        public bool CanMoveTo(Location location) => availableMoves.Contains(location);
    }
}
