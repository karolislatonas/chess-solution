using System.Collections.Generic;
using System.Linq;

namespace Chess.Domain.Movement.Movers
{
    public class ComposedMover : PieceMoverBase
    {
        private readonly IMover[] movers;

        public ComposedMover(params IMover[] movers)
        {
            this.movers = movers;
        }

        public override IEnumerable<Location> GetAvailableMovesFrom(Board board, Location from)
        {
            return movers
                .SelectMany(m => m.GetAvailableMovesFrom(board, from))
                .Distinct();
        }
    }
}
