using Chess.Messages.Events;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace Chess.Api.Client.Subscription.Subscribers
{
    public class SignalRMoveSubscriber : ISubscriber
    {
        private readonly HubConnection connection;

        public SignalRMoveSubscriber(
            HubConnection connection, 
            Action<PieceMovedEvent> pieceMovedHandler)
        {
            this.connection = connection;

            connection.On("OnPieceMoved", pieceMovedHandler);
        }

        public void Dispose()
        {
            connection?.DisposeAsync();
        }
    }
}
