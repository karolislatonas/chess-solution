using System.Collections.Generic;

namespace Chess.Domain.Movement.Movers
{
    public class CastleMover : PieceMoverBase
    {
        public override IEnumerable<Location> GetAvailableMovesFrom(Board board, Location from)
        {
            yield break;
        }
    }
}
