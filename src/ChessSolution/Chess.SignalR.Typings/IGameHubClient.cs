using Chess.Messages.Events;

namespace Chess.SignalR.Typings
{
    public interface IGameHubClient
    {
        void OnPieceMoved(PieceMovedEvent @event);

        void OnGameFinished(GameFinishedEvent @event);
    }
}
