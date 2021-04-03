namespace Chess.Messages.Events
{
    public class GameFinishedEvent
    {
        public string GameId { get; set; }

        public GameResult Result { get; set; }
    }
}
