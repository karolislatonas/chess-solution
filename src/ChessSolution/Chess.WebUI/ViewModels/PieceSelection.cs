using Chess.Domain.Movement;
using Chess.Domain.Movement.Moves;
using Chess.Domain.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace Chess.WebUI.ViewModels
{
    public class PieceSelection
    {
        private readonly Dictionary<Location, IMove> availableMovesLocations;

        public PieceSelection(Location from, IPiece piece, IEnumerable<IMove> availableMoves)
        {
            availableMovesLocations = availableMoves.ToDictionary(m => m.To, m => m);

            From = from;
            Piece = piece;
        }

        public Location From { get; }

        public IPiece Piece { get; }

        public bool CanMoveTo(Location location) => availableMovesLocations.ContainsKey(location);

        public IMove GetMoveAt(Location location) => availableMovesLocations[location];
    }
}
