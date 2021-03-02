using Chess.Domain.Movement.Moves;
using System.Linq;

namespace Chess.Domain.Movement
{
    public class MoveSequenceTranslator
    {
        private readonly MovesReplayer movesReplayer;

        public MoveSequenceTranslator()
        {
            movesReplayer = new MovesReplayer(new MovesLog());
        }

        public IMove TranslateNextMove(PieceMove pieceMove)
        {
            return TranslateNextMove(pieceMove.From, pieceMove.To);
        }

        public IMove TranslateNextMove(Location from, Location to)
        {
            var piece = movesReplayer.Board.GetPieceAt(from);

            var move = piece.Mover
                .GetAvailableMovesFrom(movesReplayer.Board, movesReplayer.MovesLog, from)
                .First(m => m.To == to);

            movesReplayer.AddMove(move);

            return move;
        }
    }
}
