namespace Chess.Messages.Events
{
    public class GameFinishedEvent : IGameEvent
    {
        public string GameId { get; set; }

        public GameResult Result { get; set; }
    }
}
