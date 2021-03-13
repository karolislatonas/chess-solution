using Chess.Messaging;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Chess.Api.Hubs
{
    public class MoveHub : Hub
    {
        private IServiceBus serviceBus;

        public MoveHub(IServiceBus serviceBus)
        {
            this.serviceBus = serviceBus;
        }

        public void SubscribeToMoves(string gameId)
        {
            ClearConnectionSubscriberIfExists();

            var subscriber = new MovesSubscriber(gameId, serviceBus, Clients.Caller);
            SetConnectionSubscriber(subscriber);
        }

        public void SubscribeToMovesFromStart(string gameId)
        {
            ClearConnectionSubscriberIfExists();

            var subscriber = new MovesSubscriber(gameId, serviceBus, Clients.Caller);
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
