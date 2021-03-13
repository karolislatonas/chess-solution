namespace Chess.Messages.Events
{
    public class PieceMovedEvent
    {
        public string GameId { get; set; }

        public LocationDto From { get; set; }

        public LocationDto To { get; set; }
    }
}
