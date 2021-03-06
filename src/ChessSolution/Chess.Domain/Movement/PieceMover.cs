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

        public bool CanPlayerMove(Board board, MovesLog movesLog, ChessColor color)
        {
            var piecesLocations = board.GetPiecesLocations(color);

            var canMove = piecesLocations.Any(pieceLoc => GetAvailableMoves(board, movesLog, pieceLoc.Location).Any());

            return canMove;
        }

        public bool IsCheckFor(Board board, ChessColor color)
        {
            var kingLocation = board.GetKingLocation(color);

            var opponentColor = color.GetOpposite();
            var opponentPiecesLocations = board.GetPiecesLocations(opponentColor);

            var kingCanBeTakeByOpponentPiece = opponentPiecesLocations
                .Any(pieceLoc => CanPieceTakeAtLocation(board, pieceLoc.Location, kingLocation));

            return kingCanBeTakeByOpponentPiece;
        }

        private IEnumerable<IMove> EnumerateAvailableMoves(Board board, MovesLog movesLog, Location from)
        {
            var mover = GetPieceMover(board, from);

            return mover
                .GetAvailableMovesFrom(board, movesLog, from)
                .Where(m => !IsCheckAfterMove(board, m));
        }

        private bool IsCheckAfterMove(Board board, IMove move)
        {
            var movingColor = board.GetPieceAt(move.From).Color;

            var newBoard = board.GetCopy();

            newBoard.ApplyMove(move);

            return IsCheckFor(newBoard, movingColor);
        }

        private bool CanPieceTakeAtLocation(Board board, Location moveFrom, Location takeAt)
        {
            var mover = GetPieceMover(board, moveFrom);

            return mover.CanTakeAt(board, moveFrom, takeAt);
        }

        private IMover GetPieceMover(Board board, Location from)
        {
            var piece = board.GetPieceAt(from);

            return piece.Mover;
        }
    }
}
