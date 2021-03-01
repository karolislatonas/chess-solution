using System;

namespace Chess.Domain.Movement.Moves
{
    public class CastleMove : MoveBase
    {
        public CastleMove(Location from, Location to) : 
            base(from, to)
        {

        }

        public override void ApplyChanges(Board board)
        {
            var rookMoveDirection = Math.Sign((from - to).Column);
            var rookFromColumn = rookMoveDirection < 0 ? 8 : 1;

            var rookFrom = new Location(from.Row, rookFromColumn);
            var rookTo = to.AddColumns(rookMoveDirection);

            board.MovePieceFromTo(from, to);
            board.MovePieceFromTo(rookFrom, rookTo);
        }
    }
}
