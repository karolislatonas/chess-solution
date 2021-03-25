using Chess.Api.Notification.Notifiers;
using Chess.Data;
using Chess.Messaging;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;

namespace Chess.Api.Notification
{
    public class NotificationHandler : IDisposable
    {
        private readonly IServiceBus serviceBus;
        private readonly IMovesRepository movesRepository;
        private readonly IClientProxy client;

        private readonly ConcurrentBag<INotifier> notifiers = new ConcurrentBag<INotifier>();

        public NotificationHandler(IServiceBus serviceBus, IMovesRepository movesRepository, IClientProxy client)
        {
            this.serviceBus = serviceBus;
            this.movesRepository = movesRepository;
            this.client = client;
        }

        public void SubscribeToMoves(string gameId)
        {
            var notifier = new MovesNotifier(gameId, serviceBus, client);

            HandleNewNotifier(notifier);
        }

        public void SubscribeToMovesFrom(string gameId, int fromSequenceNumber)
        {
            var subscriber = new MovesFromNotifier(gameId, fromSequenceNumber, serviceBus, movesRepository, client);

            HandleNewNotifier(subscriber);
        }

        public void Dispose()
        {
            foreach (var subscriber in notifiers)
                subscriber.Dispose();
        }

        private void HandleNewNotifier(INotifier notifier)
        {
            notifiers.Add(notifier);

            notifier.StartAsync();
        }
    }
}
