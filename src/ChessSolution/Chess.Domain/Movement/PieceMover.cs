namespace Chess.Domain.Movement
{
    public class PieceMover
    {
        private readonly Board board;
        private readonly MovesLog movesLog;

        public PieceMover(Board board, MovesLog movesLog)
        {
            this.board = board;
            this.movesLog = movesLog;
        }

        public PieceMove GetPieceMove(Location from, Location to)
        {
            return movesLog.GetNextMove(from, to);
        }
    }
}
