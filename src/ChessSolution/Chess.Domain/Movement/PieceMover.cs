namespace Chess.Domain.Movement
{
    public class PieceMover
    {
        private readonly MovesLog movesLog;

        public PieceMover(MovesLog movesLog)
        {
            this.movesLog = movesLog;
        }

        public PieceMove GetPieceMove(Location from, Location to)
        {
            return movesLog.GetNextMove(from, to);
        }
    }
}
