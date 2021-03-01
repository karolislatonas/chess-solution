namespace Chess.Domain.Movement.Moves
{
    public class SimpleMove : MoveBase
    {
        public SimpleMove(Location from, Location to) : 
            base(from, to)
        {

        }

        public override void ApplyChanges(Board board)
        {
            board.MovePieceFromTo(from, to);
        }
    }
}
