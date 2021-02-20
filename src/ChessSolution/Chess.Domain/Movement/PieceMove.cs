namespace Chess.Domain.Movement
{
    public class PieceMove
    {
        public PieceMove(int sequenceNumber, Location from, Location to)
        {
            SequenceNumber = sequenceNumber;
            From = from;
            To = to;
        }

        public int SequenceNumber { get; }

        public Location From { get; }

        public Location To { get; }
    }
}
