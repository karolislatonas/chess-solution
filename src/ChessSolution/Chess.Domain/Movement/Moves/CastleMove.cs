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
            var rookMoveDirection = Math.Sign((From - To).Column);
            var rookFromColumn = rookMoveDirection < 0 ? 8 : 1;

            var rookFrom = new Location(rookFromColumn, From.Row);
            var rookTo = To.AddColumns(rookMoveDirection);

            board.MovePieceFromTo(From, To);
            board.MovePieceFromTo(rookFrom, rookTo);
        }
    }
}
