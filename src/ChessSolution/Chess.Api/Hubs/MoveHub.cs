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
        private readonly IMovesRepository moveRepository;

        public MoveHub(IServiceBus serviceBus, IMovesRepository moveRepository)
        {
            this.serviceBus = serviceBus;
            this.moveRepository = moveRepository;
        }

        public void SubscribeToMoves(string gameId)
        {
            ClearConnectionSubscriberIfExists();

            var subscriber = new MovesSubscriber(gameId, serviceBus, Clients.Caller);

            SetConnectionSubscriber(subscriber);
        }

        public void SubscribeToMovesFrom(string gameId, int fromSequenceNumber)
        {
            ClearConnectionSubscriberIfExists();

            var subscriber = new MovesSubscriber(gameId, serviceBus, Clients.Caller);

            var moves = moveRepository.GetMovesFromSequence(gameId, fromSequenceNumber);

            SetConnectionSubscriber(subscriber);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ClearConnectionSubscriberIfExists();

            await Task.CompletedTask;
        }

        private void SetConnectionSubscriber(MovesSubscriber subscriber)
        {
            Context.Items[Context.ConnectionId] = subscriber;
        }

        private void ClearConnectionSubscriberIfExists()
        {
            if (!Context.Items.TryGetValue(Context.ConnectionId, out var value))
            {
                return;
            }

            var subscriber = value as MovesSubscriber;
            subscriber?.Unsubscribe();

            Context.Items.Remove(Context.ConnectionId);
        }
    }
}
