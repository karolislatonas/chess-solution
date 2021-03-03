using Chess.Domain.Extensions;
using Chess.Domain.Movement.Movers;
using Chess.Domain.Movement.Moves;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement
{
    public class PieceMover
    {
        public void EnsureIsValidMove(Board board, MovesLog movesLog, Location from, Location to)
        {
            var canMoveTo = EnumerateAvailableMoves(board, movesLog, from).Any(l => l.To == to);

            if (!canMoveTo)
            {
                throw new Exception("Invalid move");
            }
        }

        public IMove[] GetAvailableMoves(Board board, MovesLog movesLog, Location from)
        {
            return EnumerateAvailableMoves(board, movesLog, from).ToArray();
        }

        public bool IsCheckMateFor(Board board, MovesLog movesLog, ChessColor color)
        {
            var piecesLocations = board.GetPiecesLocations(color);

            var canMove = piecesLocations.Any(l => GetAvailableMoves(board, movesLog, l).Any());

            return !canMove;
        }

        private bool IsCheckAfterMove(Board board, MovesLog movesLog, IMove move)
        {
            var movingColor = board.GetPieceAt(move.From).Color;

            var newBoard = board.GetCopy();

            newBoard.ApplyMove(move);

            return IsCheckFor(newBoard, movesLog, movingColor);
        }

        public bool IsCheckFor(Board board, MovesLog movesLog, ChessColor color)
        {
            var kingLocation = board.GetKingLocation(color);

            var opponentColor = color.GetOpposite();
            var opponentPiecesLocations = board.GetPiecesLocations(opponentColor);

            var kingCanBeTakeByOpponentPiece = opponentPiecesLocations
                .Any(loc => CanPieceTakeAtLocation(board, movesLog, loc, kingLocation));

            return kingCanBeTakeByOpponentPiece;
        }

        private bool CanPieceTakeAtLocation(Board board, MovesLog movesLog, Location moveFrom, Location takeAt)
        {
            var piece = board.GetPieceAt(moveFrom);

            var availableTakes = piece.Mover
                .GetAvailableMovesFrom(board, movesLog, moveFrom)
                .OfType<TakeMove>();

            return availableTakes.Any(l => l.To == takeAt);
        }

        private IEnumerable<IMove> EnumerateAvailableMoves(Board board, MovesLog movesLog, Location from)
        {
            var mover = GetPieceMover(board, from);

            return mover
                .GetAvailableMovesFrom(board, movesLog, from)
                .Where(m => !IsCheckAfterMove(board, movesLog, m));
        }

        private IMover GetPieceMover(Board board, Location from)
        {
            var piece = board.GetPieceAt(from);

            return piece.Mover;
        }
    }
}
