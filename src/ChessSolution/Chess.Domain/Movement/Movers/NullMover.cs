
using System.Collections.Generic;

namespace Chess.Domain.Movement.Movers
{
    public class NullMover : IMover
    {
        public IEnumerable<Location> GetAvaialbleMovesFrom(Board board, Location location)
        {
            yield break;
        }

        public bool CanMoveTo(Board board, Location from, Location to)
        {
            return false;
        }
    }
}
