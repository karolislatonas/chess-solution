using Chess.Domain.Movement.Moves;
using System.Collections.Generic;

namespace Chess.Domain.Movement.Movers
{
    public interface IMover
    {
        IEnumerable<IMove> GetAvailableMovesFrom(Board board, MovesLog movesLog, Location from);

        bool CanTakeAt(Board board, Location from, Location takeAt);
    }
}
