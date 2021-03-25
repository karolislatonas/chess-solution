using Chess.Messages.Events;

namespace Chess.SignalR.Typings
{
    public interface IMoveHubClient
    {
        void OnPieceMoved(PieceMovedEvent @event);
    }
}
