using Chess.Api.Client.Subscription.Subscribers;
using Chess.Messages.Events;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chess.Api.Client.Subscription
{
    public class SignalRSubscribtionProvider : ISubscriptionProvider
    {
        private readonly string url;

        public SignalRSubscribtionProvider(string url)
        {
            this.url = url;
        }

        public async Task<ISubscriber> SubscribeAsync(string gameId, Action<PieceMovedEvent> handler)
        {
            var connection = CreateConnection();
            await connection.StartAsync();

            var subscriber = new SignalRMoveSubscriber(connection, handler);

            await connection.SendAsync("SubscribeToMoves", gameId);

            return subscriber;
        }

        public async Task<ISubscriber> SubscribeAsync(string gameId, int fromSequenceNumber, Action<PieceMovedEvent> handler)
        {
            var connection = CreateConnection();
            await connection.StartAsync();

            var subscriber = new SignalRMoveSubscriber(connection, handler);

            await connection.SendAsync("SubscribeToMovesFrom", gameId, fromSequenceNumber);

            return subscriber;
        }

        private HubConnection CreateConnection()
        {
            return new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();
        }
    }
}
