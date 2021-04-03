using Chess.Api.Client.Subscription.Subscribers;
using Chess.Messages.Events;
using Chess.SignalR.Typings;
using Microsoft.AspNetCore.SignalR.Client;
using System;
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

        public async Task<ISubscriber> SubscribeAsync(string gameId, int fromSequenceNumber, 
            Action<PieceMovedEvent> moveHandler, 
            Action<GameFinishedEvent> gameFinishedHandler)
        {
            var connection = CreateConnection();
            await connection.StartAsync();

            var subscriber = new SignalRGameSubscriber(connection, moveHandler, gameFinishedHandler);

            await connection.SendAsync(nameof(IGameHub.SubscribeToGameEvents), gameId, fromSequenceNumber);

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
