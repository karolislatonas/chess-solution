using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public abstract class PieceMoverBase : IMover
    {
        public bool CanMoveTo(Board board, Location from, Location to)
        {
            return GetAvailableMovesFrom(board, from).Any(l => l == to);
        }

        public abstract IEnumerable<Location> GetAvailableMovesFrom(Board board, Location from);
    }
}
