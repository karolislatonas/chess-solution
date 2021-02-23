using System.Collections.Generic;

namespace Chess.Domain.Movement.Movers
{
    public class BishopMover : DirectionalMover
    {
        protected override IEnumerable<Location> GetPossibleMoveDirections()
        {
            yield return new Location(1, 1);
            yield return new Location(-1, -1);
            yield return new Location(-1, 1);
            yield return new Location(1, -1);
        }
    }
}
