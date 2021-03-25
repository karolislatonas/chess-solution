using Chess.Messages.Events;
using Chess.SignalR.Typings;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Chess.Api.Client.Subscription.Subscribers
{
    public class SignalRMovesSubscriber : ISubscriber
    {
        private readonly HubConnection connection;
        private readonly Action<PieceMovedEvent> pieceMovedHandler;

        public SignalRMovesSubscriber(
            HubConnection connection, 
            Action<PieceMovedEvent> pieceMovedHandler)
        {
            this.connection = connection;
            this.pieceMovedHandler = pieceMovedHandler;

            connection.On(nameof(IMoveHubClient.OnPieceMoved), pieceMovedHandler);
        }

        public async ValueTask DisposeAsync()
        {
            await connection.DisposeAsync();
        }
    }
}
