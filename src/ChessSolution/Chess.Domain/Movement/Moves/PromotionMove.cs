using Chess.Domain.Pieces;

namespace Chess.Domain.Movement.Moves
{
    public class PromotionMove : MoveBase
    {
        private readonly IPiece promoteToPiece;

        public PromotionMove(Location from, Location to, IPiece promoteToPiece) : 
            base(from, to)
        {
            this.promoteToPiece = promoteToPiece;
        }

        public override void ApplyChanges(Board board)
        {
            board.RemovePieceFrom(From);
            board.AddPieceAt(promoteToPiece, To);
        }
    }
}
