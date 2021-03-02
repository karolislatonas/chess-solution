namespace Chess.Domain.Movement.Moves
{
    public class PassedPawnTakeMove : MoveBase
    {
        public PassedPawnTakeMove(Location from, Location to) : base(from, to)
        {

        }

        public override void ApplyChanges(Board board)
        {
            var passedPawnLocation = new Location(To.Column, From.Row);

            board.RemovePieceFrom(passedPawnLocation);
            board.MovePieceFromTo(From, To);
        }
    }
}
