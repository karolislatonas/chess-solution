﻿using Chess.Domain.Extensions;
using Chess.Domain.Movement.Moves;
using Chess.Domain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class CastleMover : IMover
    {
        private static readonly IEnumerable<IMove> NoMoves = Enumerable.Empty<IMove>();

        public IEnumerable<IMove> GetAvailableMovesFrom(Board board, MovesLog movesLog, Location from)
        {
            var king = board.GetPieceAt<King>(from);

            var kingInitialPosition = GetInitialKingPosition(king.Color);
            
            var kingWasMoved = WasMoveFrom(kingInitialPosition, movesLog);

            if (kingWasMoved)
                return NoMoves;

            var rookLocationsForCastle = GetRookLocationsForCastle(board, from);

            return rookLocationsForCastle
                .Where(rookLoc => !WasMoveFrom(rookLoc, movesLog))
                .Select(rookLoc => GetCastlePassingLocation(from, rookLoc))
                .Where(locs => CanKingPassLocations(board, from, king.Color, locs))
                .Select(locs => locs.Last())
                .Select(loc => new CastleMove(from, loc));
        }

        public bool CanTakeAt(Board board, Location from, Location takeAt)
        {
            return false;
        }

        private Location GetInitialKingPosition(ChessColor chessColor)
        {
            return chessColor switch
            {
                ChessColor.White => new Location(5, 1),
                ChessColor.Black => new Location(5, 8),
                _ => throw new ArgumentException($"Cannot determine initial king location for {chessColor}")
            };
        }

        private bool CanKingPassLocations(Board board, Location kingLocation, ChessColor kingColor, Location[] passingLocations)
        {
            var opponentPiecesLocations = board.GetPiecesLocations(kingColor.GetOpposite());

            var boardWithoutKing = board.GetCopy();
            boardWithoutKing.RemovePieceFrom(kingLocation);

            return !opponentPiecesLocations
                .Any(pl => passingLocations.Any(l => pl.Piece.Mover.CanTakeAt(boardWithoutKing, pl.Location, l)));
        }

        private Location[] GetCastlePassingLocation(Location kingLocation, Location rookLocation)
        {
            var direction = Math.Sign((rookLocation - kingLocation).Column);

            return new Location[]
            {
                kingLocation,
                kingLocation.AddColumns(direction),
                kingLocation.AddColumns(2 * direction)
            };
        }

        private IEnumerable<Location> GetRookLocationsForCastle(Board board, Location from)
        {
            var row = from.Row;

            return GetPossibleRookLocationsForCastle(row)
                .Where(board.ContainsPieceAt)
                .Where(board.IsPieceOfTypeAt<Rook>)
                .Where(l => NoPiecesBetweenColumnsInRow(board, row, from.Column, l.Column));
        }

        private IEnumerable<Location> GetPossibleRookLocationsForCastle(int row)
        {
            yield return new Location(1, row);
            yield return new Location(8, row);
        }

        private bool NoPiecesBetweenColumnsInRow(Board board, int row, int fromColumn, int toColumn)
        {
            var from = Math.Min(fromColumn, toColumn) + 1;
            var to = Math.Max(fromColumn, toColumn) - 1;

            for(var i = from; i <= to; i++)
            {
                var location = new Location(i, row);

                if (board.ContainsPieceAt(location))
                    return false;
            }

            return true;
        }

        private bool WasMoveFrom(Location from, MovesLog movesLog)
        {
            return movesLog.Any(m => m.Move.From == from);
        }
    }
}
