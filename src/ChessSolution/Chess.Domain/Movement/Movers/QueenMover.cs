using System.Collections.Generic;

namespace Chess.Domain.Movement.Movers
{
    public class QueenMover : DirectionalMover
    {
        protected override IEnumerable<Location> GetPossibleMoveDirections()
        {
            yield return new Location(1, 1);
            yield return new Location(-1, -1);
            yield return new Location(-1, 1);
            yield return new Location(1, -1);
            yield return new Location(1, 0);
            yield return new Location(0, 1);
            yield return new Location(-1, 0);
            yield return new Location(0, -1);
        }
    }
}
