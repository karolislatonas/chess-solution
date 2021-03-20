using Chess.Domain.Pieces;

namespace Chess.Domain.Movement.Moves
{
    public class PromotionMoveWrapper : IMove
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

        private PromotionMoveWrapper(IMove innerMove)
        {
            this.innerMove = innerMove;
        }

        public Location From => innerMove.From;

        public Location To => innerMove.To;

        public void ApplyChanges(Board board)
        {
            innerMove.ApplyChanges(board);

            var piece = board.GetPieceAt(To);

            board.RemovePieceFrom(To);
            board.AddPieceAt(To, new Queen(piece.Color));
        }
    }
}
