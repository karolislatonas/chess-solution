using Chess.Domain.Movement.Moves;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class ComposedMover : IMover
    {
        private readonly IMover[] movers;

        public ComposedMover(params IMover[] movers)
        {
            this.movers = movers;
        }

        public IEnumerable<IMove> GetAvailableMovesFrom(Board board, MovesLog movesLog, Location from)
        {
            return movers.SelectMany(m => m.GetAvailableMovesFrom(board, movesLog, from));
        }

        public bool CanTakeAt(Board board, Location from, Location takeAt)
        {
            return movers.Any(m => m.CanTakeAt(board, from, takeAt));
        }
    }
}
