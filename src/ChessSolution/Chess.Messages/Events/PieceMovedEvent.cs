using Chess.Shared.DataContracts;

namespace Chess.Messages.Events
{
    public class PieceMovedEvent : IGameEvent
    {
        public string GameId { get; set; }

        public PieceMoveDto PieceMove { get; set; }
    }
}
