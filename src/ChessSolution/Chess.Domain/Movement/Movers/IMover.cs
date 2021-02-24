using System.Collections.Generic;

namespace Chess.Domain.Movement.Movers
{
    public interface IMover
    {
        bool CanMoveTo(Board board, Location from, Location to);

        IEnumerable<Location> GetAvailableMovesFrom(Board board, Location from);
    }
}
