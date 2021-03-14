using Chess.Data;
using Chess.Messaging;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Chess.Api.Hubs
{
    public class MoveHub : Hub
    {
        private IServiceBus serviceBus;
        private readonly IMovesRepository movesRepository;

        public MoveHub(IServiceBus serviceBus, IMovesRepository movesRepository)
        {
            this.serviceBus = serviceBus;
            this.movesRepository = movesRepository;
        }

        public void SubscribeToMoves(string gameId)
        {
            ClearConnectionSubscriberIfExists();

            var subscriber = new MovesSubscriber(gameId, serviceBus, Clients.Caller);

            SetConnectionSubscriber(subscriber);
        }

        public async Task SubscribeToMovesFrom(string gameId, int fromSequenceNumber)
        {
            ClearConnectionSubscriberIfExists();

            var subscriber = new MovesSubcriberFrom(gameId, fromSequenceNumber, serviceBus, movesRepository, Clients.Caller);

            await subscriber.StartAsync();

            SetConnectionSubscriber(subscriber);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ClearConnectionSubscriberIfExists();

            await Task.CompletedTask;
        }

        private void SetConnectionSubscriber(ISubscriber subscriber)
        {
            Context.Items[Context.ConnectionId] = subscriber;
        }

        private void ClearConnectionSubscriberIfExists()
        {
            if (!Context.Items.TryGetValue(Context.ConnectionId, out var value))
            {
                return;
            }

            var subscriber = value as ISubscriber;
            subscriber?.Unsubscribe();

            Context.Items.Remove(Context.ConnectionId);
        }
    }
}
