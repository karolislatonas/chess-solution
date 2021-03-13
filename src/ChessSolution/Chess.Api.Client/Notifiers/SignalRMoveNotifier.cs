using Chess.Api.DataContracts;
using Chess.Messages.Events;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Chess.Api.Client.Notifiers
{
    public class SignalRMoveNotifier : IMoveNotifier
    {
        private readonly HubConnection connection;

        public SignalRMoveNotifier(string url)
        {
            connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            connection.StartAsync();
        }

        public async Task SubscribeAsync(string gameId, Action<PieceMovedEvent> handler)
        {
            connection.On("OnPieceMoved", handler);
            await connection.SendAsync("SubscribeToMoves", gameId);
        }

        public async Task SubscribeFromStartAsync(string gameId, Action<PieceMovedEvent> handler)
        {
            connection.On("OnPieceMoved", handler);
            await connection.SendAsync("SubscribeToMovesFromStart", gameId);
        }

        public void Dispose()
        {
            connection?.DisposeAsync();
        }
    }
}
