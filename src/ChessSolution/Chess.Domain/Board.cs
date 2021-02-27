using Chess.Domain.Movement;
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

        public TPiece GetPieceAt<TPiece>(Location location)
            where TPiece : IPiece
        {
            return (TPiece)GetPieceAt(location);
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

        public void ApplyMoves(MovesLog movesLog)
        {
            ApplyMoves(movesLog);
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

        private void MovePieceFromTo(Location from, Location to)
        {
            if(!pieces.TryGetValue(from, out var piece))
            {
                throw new Exception();
            }

            pieces.Remove(from);
            pieces[to] = piece;
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
