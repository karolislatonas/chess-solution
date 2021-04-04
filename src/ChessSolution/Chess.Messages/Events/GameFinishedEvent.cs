using Chess.Shared.DataContracts;

namespace Chess.Messages.Events
{
    public class GameFinishedEvent : IGameEvent
    {
        public string GameId { get; set; }

        public GameResultDto Result { get; set; }
    }
}
