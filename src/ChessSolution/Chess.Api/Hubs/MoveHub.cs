using Chess.Api.Notification;
using Chess.Data;
using Chess.Messaging;
using Chess.SignalR.Typings;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Chess.Api.Hubs
{
    public class MoveHub : Hub, IMoveHub
    {
        private readonly IServiceBus serviceBus;
        private readonly IMovesRepository movesRepository;

        public MoveHub(IServiceBus serviceBus, IMovesRepository movesRepository)
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

        public void SubscribeToMoves(string gameId)
        {
            var notificationHandler = GetNotificationHandler();

            notificationHandler.SubscribeToMoves(gameId);
        }

        public void SubscribeToMovesFrom(string gameId, int fromSequenceNumber)
        {
            var notificationHandler = GetNotificationHandler();

            notificationHandler.SubscribeToMovesFrom(gameId, fromSequenceNumber);
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
