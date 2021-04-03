using Chess.Api.Notification;
using Chess.Data;
using Chess.Messaging;
using Chess.SignalR.Typings;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Chess.Api.Hubs
{
    public class GameHub : Hub, IGameHub
    {
        private readonly IServiceBus serviceBus;
        private readonly IMovesRepository movesRepository;

        public GameHub(IServiceBus serviceBus, IMovesRepository movesRepository)
        {
            this.serviceBus = serviceBus;
            this.movesRepository = movesRepository;
        }

        public override async Task OnConnectedAsync()
        {
            AddNotificationHandlerForNewConnection();
            
            await Task.CompletedTask;
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ClearNotificationHandlerIfExists();

            await Task.CompletedTask;
        }

        public void SubscribeToGameEvents(string gameId, int fromSequenceNumber)
        {
            var notificationHandler = GetNotificationHandler();

            notificationHandler.SubscribeToEvents(gameId, fromSequenceNumber);
        }

        private void AddNotificationHandlerForNewConnection()
        {
            var notificationHanlder = new NotificationHandler(serviceBus, movesRepository, Clients.Caller);

            Context.Items[Context.ConnectionId] = notificationHanlder;
        }

        private NotificationHandler GetNotificationHandler()
        {
            return (NotificationHandler)Context.Items[Context.ConnectionId]; 
        }

        private void ClearNotificationHandlerIfExists()
        {
            if (!Context.Items.TryGetValue(Context.ConnectionId, out var value))
            {
                return;
            }

            var subscriptionHandler = (NotificationHandler)value;
            subscriptionHandler?.Dispose();

            Context.Items.Remove(Context.ConnectionId);
        }
    }
}
