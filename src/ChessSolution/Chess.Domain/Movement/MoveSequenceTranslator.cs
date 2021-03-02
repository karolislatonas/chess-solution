using Chess.Domain.Movement.Moves;
using System.Linq;

namespace Chess.Domain.Movement
{
    public class MoveSequenceTranslator
    {
        private readonly Board board;

        public MoveSequenceTranslator(Board board)
        {
            this.board = board;
        }

        public IMove TranslateNextMove(PieceMove pieceMove)
        {
            return TranslateNextMove(pieceMove.From, pieceMove.To);
        }

        public IMove TranslateNextMove(Location from, Location to)
        {
            var piece = board.GetPieceAt(from);

            var move = piece.Mover
                .GetAvailableMovesFrom(board, from)
                .First(m => m.To == to);

            board.ApplyMove(move);

            return move;
        }
    }
}
