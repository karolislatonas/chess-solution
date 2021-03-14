using Chess.Domain.Pieces;

namespace Chess.Domain.Movement.Moves
{
    public class PromotionMoveWrapper : MoveBase
    {
        private readonly IMove innerMove;

        public PromotionMoveWrapper(TakeMove innerMove) :
            this((IMove)innerMove)
        {
            
        }

        public PromotionMoveWrapper(SimpleMove innerMove) :
            this((IMove)innerMove)
        {
            
        }

        private PromotionMoveWrapper(IMove innerMove) : 
            base(innerMove.From, innerMove.To)
        {
            this.innerMove = innerMove;
        }

        public override void ApplyChanges(Board board)
        {
            innerMove.ApplyChanges(board);

            var piece = board.GetPieceAt(To);

            board.RemovePieceFrom(To);
            board.AddPieceAt(To, new Queen(piece.Color));
        }
    }
}
