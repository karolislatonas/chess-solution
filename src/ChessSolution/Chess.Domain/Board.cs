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
            return CreatePieciesLineAt(0)
                .Concat(CreatePawnsLineAt(1))
                .Concat(CreatePieciesLineAt(7))
                .Concat(CreatePawnsLineAt(6))
                .ToDictionary(x => x.location, x => x.piece);
        }

        private static IEnumerable<(Location location, IPiece piece)> CreatePieciesLineAt(int lineIndex)
        {
            yield return (LocationAt(0, lineIndex), new Rook());
            yield return (LocationAt(1, lineIndex), new Knight());
            yield return (LocationAt(2, lineIndex), new Bishop());
            yield return (LocationAt(3, lineIndex), new Queen());
            yield return (LocationAt(4, lineIndex), new King());
            yield return (LocationAt(5, lineIndex), new Bishop());
            yield return (LocationAt(6, lineIndex), new Knight());
            yield return (LocationAt(7, lineIndex), new Rook());
        }

        private static IEnumerable<(Location location, IPiece piece)> CreatePawnsLineAt(int lineIndex)
        {
            yield return (LocationAt(0, lineIndex), new Pawn());
            yield return (LocationAt(1, lineIndex), new Pawn());
            yield return (LocationAt(2, lineIndex), new Pawn());
            yield return (LocationAt(3, lineIndex), new Pawn());
            yield return (LocationAt(4, lineIndex), new Pawn());
            yield return (LocationAt(5, lineIndex), new Pawn());
            yield return (LocationAt(6, lineIndex), new Pawn());
            yield return (LocationAt(7, lineIndex), new Pawn());
        }
    }
}
