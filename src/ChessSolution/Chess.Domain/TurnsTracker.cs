using Chess.Domain.Movement;

namespace Chess.Domain
{
    public class TurnsTracker
    {
        private readonly MovesLog movesLog;

        public TurnsTracker(MovesLog movesLog)
        {
            this.movesLog = movesLog;
        }

        public bool IsTurnFor(ChessColor chessColor)
        {
            return GetCurrentPlayerToMove() == chessColor;
        }

        public ChessColor GetCurrentPlayerToMove()
        {
            var latestMove = movesLog.LatestMove;

            if (latestMove == null)
                return ChessColor.White;

            return latestMove.SequenceNumber % 2 == 0 ?
                ChessColor.White :
                ChessColor.Black;
        }
    }
}
