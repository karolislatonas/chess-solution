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

        public void SubscribeToEvents(string gameId, int fromSequenceNumber)
        {
            var movesSubscriber = new MovesNotifier(gameId, fromSequenceNumber, serviceBus, movesRepository, client);
            var gameFinishedSubscriber = new GameNotifier(gameId, serviceBus, client);

            HandleNewNotifier(movesSubscriber);
            HandleNewNotifier(gameFinishedSubscriber);
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
