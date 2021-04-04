namespace Chess.Shared.DataContracts
{
    public class PieceMoveDto
    {
        public int SequenceNumber { get; set; }

        public LocationDto From { get; set; }

        public LocationDto To { get; set; }
    }
}
