using Chess.Domain.Movement.Moves;
using Chess.Domain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class CastleMover : IMover
    {
        public IEnumerable<IMove> GetAvailableMovesFrom(Board board, Location from)
        {
            if (!IsInitialKingPosition(from))
                return Enumerable.Empty<IMove>();

            var king = board.GetPieceAt<King>(from);

            var rookLocationForCastle = GetRookLocationsForCastle(board, from);

            return rookLocationForCastle
                .Select(rl => GetCastleMoveLocation(from, rl))
                .Select(l => CreateMove(from, l));
        }

        private bool IsInitialKingPosition(Location location)
        {
            return location == new Location(5, 1) ||
                location == new Location(5, 8);
        }

        private Location GetCastleMoveLocation(Location kingLocation, Location rookLocation)
        {
            var direction = Math.Sign((rookLocation - kingLocation).Column);

            return kingLocation.AddColumns(2 * direction);
        }

        private IEnumerable<Location> GetRookLocationsForCastle(Board board, Location from)
        {
            var row = from.Row;

            return GetPossibleRookLocationsForCastle(row)
                .Where(board.ContainsPieceAt)
                .Where(l => board.IsPieceOfTypeAt<Rook>(l))
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

        private IMove CreateMove(Location from, Location to)
        {
            return new CastleMove(from, to);
        }
    }
}
