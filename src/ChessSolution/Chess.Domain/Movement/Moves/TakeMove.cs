namespace Chess.Domain.Movement.Moves
{
    public class TakeMove : MoveBase
    {
        public TakeMove(Location from, Location to) : 
            base(from, to)
        {

        }

        public override void ApplyChanges(Board board)
        {
            board.RemovePieceFrom(To);
            board.MovePieceFromTo(From, To);
        }
    }
}
