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

        public IPiece GetPieceAt(Location location)
        {
            if(pieces.TryGetValue(location, out var piece))
            {
                return piece;
            }

            return null;
        }

        public void ApplyMoves(IEnumerable<PieceMove> pieceMoves)
        {
            foreach (var pieceMove in pieceMoves)
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
            return CreatePieciesInRow(1)
                .Concat(CreatePawnsInRow(2))
                .Concat(CreatePieciesInRow(8))
                .Concat(CreatePawnsInRow(7))
                .ToDictionary(x => x.location, x => x.piece);
        }

        private static IEnumerable<(Location location, IPiece piece)> CreatePieciesInRow(int lineIndex)
        {
            yield return (LocationAt(1, lineIndex), new Rook());
            yield return (LocationAt(2, lineIndex), new Knight());
            yield return (LocationAt(3, lineIndex), new Bishop());
            yield return (LocationAt(4, lineIndex), new Queen());
            yield return (LocationAt(5, lineIndex), new King());
            yield return (LocationAt(6, lineIndex), new Bishop());
            yield return (LocationAt(7, lineIndex), new Knight());
            yield return (LocationAt(8, lineIndex), new Rook());
        }

        private static IEnumerable<(Location location, IPiece piece)> CreatePawnsInRow(int lineIndex)
        {
            yield return (LocationAt(1, lineIndex), new Pawn());
            yield return (LocationAt(2, lineIndex), new Pawn());
            yield return (LocationAt(3, lineIndex), new Pawn());
            yield return (LocationAt(4, lineIndex), new Pawn());
            yield return (LocationAt(5, lineIndex), new Pawn());
            yield return (LocationAt(6, lineIndex), new Pawn());
            yield return (LocationAt(7, lineIndex), new Pawn());
            yield return (LocationAt(8, lineIndex), new Pawn());
        }
    }
}
