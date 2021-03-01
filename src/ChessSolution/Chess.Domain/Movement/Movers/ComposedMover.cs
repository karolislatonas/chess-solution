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

        public IMove CreateMove(Board board, Location from, Location to)
        {
            var mover = movers.First(m => m.GetAvailableMovesFrom(board, from).Any(l => l == to));

            return mover.CreateMove(board, from, to);
        }

        public IEnumerable<Location> GetAvailableMovesFrom(Board board, Location from)
        {
            return movers
                .SelectMany(m => m.GetAvailableMovesFrom(board, from))
                .Distinct();
        }
    }
}
