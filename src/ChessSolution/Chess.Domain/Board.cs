using Chess.Domain.Extensions;
using Chess.Domain.Movement;
using Chess.Domain.Movement.Moves;
using Chess.Domain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using static Chess.Domain.Movement.Location;

namespace Chess.Domain
{
    public class Board
    {
        private readonly Dictionary<Location, PieceLocation> pieces;

        private Board(Dictionary<Location, PieceLocation> pieces)
        {
            this.pieces = pieces;
        }

        public Board()
        {
            pieces = GetInitialPieciesLocations().ToDictionary(kvp => kvp.Key, kvp => new PieceLocation(kvp.Value, kvp.Key));
            BoardDetails = new BoardDetails();
        }

        public BoardDetails BoardDetails { get; }

        public bool IsWithinBoard(Location location)
        {
            return 1 <= location.Column && location.Column <= 8 &&
                   1 <= location.Row && location.Row <= 8;
        }

        public bool IsPieceOfColorAt(Location location, ChessColor color)
        {
            var piece = GetPieceAt(location);

            return piece?.Color == color;
        }

        public IEnumerable<PieceLocation> GetPiecesLocations(ChessColor color)
        {
            return pieces
                .Values
                .Where(p => p.Piece.Color == color);
        }

        public TPiece GetPieceAt<TPiece>(Location location)
            where TPiece : IPiece
        {
            return (TPiece)GetPieceAt(location);
        }

        public bool IsPieceOfTypeAt<TPiece>(Location location)
            where TPiece : IPiece
        {
            return GetPieceAt(location) is TPiece;
        }

        public bool ContainsPieceAt(Location location)
        {
            return pieces.ContainsKey(location);
        }

        public PieceLocation GetPieceLocationAt(Location location)
        {
            if (pieces.TryGetValue(location, out var piece))
            {
                return piece;
            }

            return null;
        }

        public IPiece GetPieceAt(Location location)
        {
            var pieceLocation = GetPieceLocationAt(location);

            return pieceLocation?.Piece;
        }

        public Location GetKingLocation(ChessColor color)
        {
            return pieces
                .Values
                .Where(p => p.Piece.Color == color)
                .Single(p => p.Piece is King)
                .Location;
        }

        public void ApplyMoves(IEnumerable<IMove> moves)
        {
            foreach (var pieceMove in moves)
                ApplyMove(pieceMove);
        }

        public void ApplyMove(IMove move)
        {
            move.ApplyChanges(this);
        }

        public void MovePieceFromTo(Location from, Location to)
        {
            var pieceLocation = GetPieceLocationAt(from);
            
            if(pieceLocation == null)
                throw new Exception($"Where is no piece at Row-{from.Row}, Col-{from.Column}.");

            var newPieceLocation = pieceLocation.WithNewLocation(to);

            pieces.Remove(from);
            pieces.Add(to, newPieceLocation);
        }

        public void AddPieceAt(Location at, IPiece piece)
        {
            pieces.Add(at, new PieceLocation(piece, at));
        }

        public void RemovePieceFrom(Location from)
        {
            var pieceToRemove = GetPieceAt(from);

            if (pieceToRemove == null)
                throw new Exception($"Where is no piece at Row-{from.Row}, Col-{from.Column}.");

            pieces.Remove(from);
        }

        public Board GetCopy()
        {
            return new Board(pieces.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        private static Dictionary<Location, IPiece> GetInitialPieciesLocations()
        {
            return CreatePieciesInRow(1, ChessColor.White)
                .Concat(CreatePawnsInRow(2, ChessColor.White))
                .Concat(CreatePieciesInRow(8, ChessColor.Black))
                .Concat(CreatePawnsInRow(7, ChessColor.Black))
                .ToDictionary(x => x.location, x => x.piece);
        }

        private static IEnumerable<(Location location, IPiece piece)> CreatePieciesInRow(int lineIndex, ChessColor color)
        {
            yield return (LocationAt(1, lineIndex), new Rook(color));
            yield return (LocationAt(2, lineIndex), new Knight(color));
            yield return (LocationAt(3, lineIndex), new Bishop(color));
            yield return (LocationAt(4, lineIndex), new Queen(color));
            yield return (LocationAt(5, lineIndex), new King(color));
            yield return (LocationAt(6, lineIndex), new Bishop(color));
            yield return (LocationAt(7, lineIndex), new Knight(color));
            yield return (LocationAt(8, lineIndex), new Rook(color));
        }

        private static IEnumerable<(Location location, IPiece piece)> CreatePawnsInRow(int lineIndex, ChessColor color)
        {
            yield return (LocationAt(1, lineIndex), new Pawn(color));
            yield return (LocationAt(2, lineIndex), new Pawn(color));
            yield return (LocationAt(3, lineIndex), new Pawn(color));
            yield return (LocationAt(4, lineIndex), new Pawn(color));
            yield return (LocationAt(5, lineIndex), new Pawn(color));
            yield return (LocationAt(6, lineIndex), new Pawn(color));
            yield return (LocationAt(7, lineIndex), new Pawn(color));
            yield return (LocationAt(8, lineIndex), new Pawn(color));
        }
    }
}
