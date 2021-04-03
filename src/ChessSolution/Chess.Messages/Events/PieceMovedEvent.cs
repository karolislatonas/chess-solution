namespace Chess.Messages.Events
{
    public class PieceMovedEvent : IGameEvent
    {
        public string GameId { get; set; }

        public int SequenceNumber { get; set; }

        public LocationDto From { get; set; }

        public LocationDto To { get; set; }
    }
}
