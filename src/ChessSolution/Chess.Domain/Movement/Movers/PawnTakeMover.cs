using Chess.Domain.Extensions;
using Chess.Domain.Movement.Moves;
using Chess.Domain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class PawnTakeMover : IMover
    {
        private static IEnumerable<IMove> NoMoves = Enumerable.Empty<IMove>();

        public IEnumerable<IMove> GetAvailableMovesFrom(Board board, MovesLog movesLog, Location from)
        {
            return GetRegularTakeMoves(board, from)
                .Concat(GetPassingPawnTakeMoves(board, movesLog, from));
        }

        private IEnumerable<IMove> GetRegularTakeMoves(Board board, Location from)
        {
            var piece = board.GetPieceAt<Pawn>(from);

            return piece
                 .TakeDirections
                 .Select(d => from.Add(d))
                 .Where(l => board.IsPieceOfColor(l, piece.Color.GetOpposite()))
                 .Select(l => new TakeMove(from, l));
        }

        private IEnumerable<IMove> GetPassingPawnTakeMoves(Board board, MovesLog movesLog, Location from)
        {
            if (!PassingPawnCanBeTaken(board, movesLog, from))
                return NoMoves;

            var lastMove = movesLog.LastMove.Move;
            var opponentMovingDirection = Math.Sign((lastMove.To - lastMove.From).Row);
            var passedLocationByPawn = lastMove.To.AddRows(-opponentMovingDirection);

            var pawn = board.GetPieceAt<Pawn>(from);
            var takeLocations = pawn.TakeDirections.Select(d => from.Add(d));

            return takeLocations
                .Where(l => l == passedLocationByPawn)
                .Select(l => new PassedPawnTakeMove(from, l));

        }

        private bool PassingPawnCanBeTaken(Board board, MovesLog movesLog, Location from)
        {
            if (movesLog.TotalMoves == 0)
                return false;

            var lastMove = movesLog.LastMove.Move;

            var isLastMoveByPawn = board.GetPieceAt(lastMove.To) is Pawn;

            if (!isLastMoveByPawn)
                return false;

            var isDoubleJump = Math.Abs((lastMove.To - lastMove.From).Row) > 1;

            if (!isDoubleJump)
                return false;

            return true;
        }
    }
}
