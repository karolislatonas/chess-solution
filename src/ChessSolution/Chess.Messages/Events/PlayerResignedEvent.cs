namespace Chess.Messages.Events
{
    public class PlayerResignedEvent
    {
        public string GameId { get; set; }

        public string ResignedPlayerId { get; set; }
    }
}
