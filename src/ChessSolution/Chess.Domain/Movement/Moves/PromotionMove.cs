using Chess.Domain.Pieces;

namespace Chess.Domain.Movement.Moves
{
    public class PromotionMove : MoveBase
    {
        public PromotionMove(Location from, Location to) : 
            base(from, to)
        {
            
        }

        public override void ApplyChanges(Board board)
        {
            var piece = board.GetPieceAt(From);

            board.RemovePieceFrom(From);
            board.AddPieceAt(To, new Queen(piece.Color));
        }
    }
}
