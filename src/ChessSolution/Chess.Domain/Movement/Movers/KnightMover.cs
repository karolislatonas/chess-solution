using System.Collections.Generic;

namespace Chess.Domain.Movement.Movers
{
    public class KnightMover : SingleJumpMover
    {
        protected override IEnumerable<Location> GetPossibleMoveDirections()
        {
            yield return new Location(2, 1);
            yield return new Location(2, -1);
            yield return new Location(-2, 1);
            yield return new Location(-2, -1);
            yield return new Location(1, 2);
            yield return new Location(-1, 2);
            yield return new Location(1, -2);
            yield return new Location(-1, -2);
        }
    }
}
