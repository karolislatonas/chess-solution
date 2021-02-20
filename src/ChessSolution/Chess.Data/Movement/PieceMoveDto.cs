namespace Chess.Data.Movement
{
    public class PieceMoveDto
    {
        public int GameId { get; set; }

        public int FromX { get; set; }

        public int FromY { get; set; }

        public int ToX { get; set; }

        public int ToY { get; set; }

        public int SequenceNumber { get; set; }
    }
}
