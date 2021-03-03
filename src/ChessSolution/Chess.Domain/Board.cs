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
        private readonly Dictionary<Location, IPiece> pieces;

        private Board(Dictionary<Location, IPiece> pieces)
        {
            this.pieces = pieces;
        }

        public Board()
        {
            pieces = GetInitialPieciesLocations();
        }

        public bool IsWithinBoard(Location location)
        {
            return 1 <= location.Column && location.Column <= 8 &&
                   1 <= location.Row && location.Row <= 8;
        }

        public bool IsPieceOfColor(Location location, ChessColor color)
        {
            var piece = GetPieceAt(location);

            return piece?.Color == color;
        }

        public IEnumerable<Location> GetPiecesLocations(ChessColor color)
        {
            return pieces
                .Where(p => p.Value.Color == color)
                .Select(p => p.Key);
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

        public IPiece GetPieceAt(Location location)
        {
            if(pieces.TryGetValue(location, out var piece))
            {
                return piece;
            }

            return null;
        }

        public Location GetKingLocation(ChessColor color)
        {
            return pieces
                .Where(kvp => kvp.Value.Color == color)
                .Single(kvp => kvp.Value is King)
                .Key;
        }

        public void ApplyMoves(MovesLog movesLog)
        {
            ApplyMoves(movesLog as IEnumerable<PieceMove>);
        }

        public void ApplyMoves(IEnumerable<PieceMove> moves)
        {
            foreach (var pieceMove in moves)
                ApplyMove(pieceMove);
        }

        public void ApplyMove(PieceMove pieceMove)
        {
            MovePieceFromTo(pieceMove.From, pieceMove.To);
        }

        public void ApplyMove(IMove move)
        {
            move.ApplyChanges(this);
        }

        public void MovePieceFromTo(Location from, Location to)
        {
            var pieceToMove = GetPieceAt(from);
            
            if(pieceToMove == null)
                throw new Exception($"Where is no piece at Row-{from.Row}, Col-{from.Column}.");

            pieces.Remove(from);
            pieces.Add(to, pieceToMove);
        }

        public void AddPieceAt(IPiece piece, Location at)
        {
            pieces.Add(at, piece);
        }

        public void RemovePieceFrom(Location from)
        {
            var pieceToRemove = GetPieceAt(from);

            if (pieceToRemove == null)
                throw new Exception($"Where is no piece at Row-{from.Row}, Col-{from.Column}.");

            if(pieceToRemove is King)
                throw new Exception($"King cannot be removed from board.");

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
