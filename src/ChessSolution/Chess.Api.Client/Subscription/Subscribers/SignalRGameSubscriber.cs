using Chess.Messages.Events;
using Chess.SignalR.Typings;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Chess.Api.Client.Subscription.Subscribers
{
    public class SignalRGameSubscriber : ISubscriber
    {
        private readonly HubConnection connection;
        private readonly Action<PieceMovedEvent> pieceMovedHandler;

        public SignalRGameSubscriber(
            HubConnection connection, 
            Action<PieceMovedEvent> pieceMovedHandler,
            Action<GameFinishedEvent> gameFinishedHandler)
        {
            this.connection = connection;
            this.pieceMovedHandler = pieceMovedHandler;

            connection.On(nameof(IGameHubClient.OnPieceMoved), pieceMovedHandler);
            connection.On(nameof(IGameHubClient.OnGameFinished), gameFinishedHandler);
        }

        public async ValueTask DisposeAsync()
        {
            await connection.DisposeAsync();
        }
    }
}
