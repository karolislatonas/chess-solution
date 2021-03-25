using Chess.Messages.Events;
using Chess.SignalR.Typings;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Chess.Api.Client.Subscription.Subscribers
{
    public class SignalRMovesSubscriber : ISubscriber
    {
        private readonly HubConnection connection;
        private readonly Action<PieceMovedEvent> pieceMovedHandler;
        private readonly BlockingCollection<PieceMovedEvent> blockingCollection;

        public SignalRMovesSubscriber(
            HubConnection connection, 
            Action<PieceMovedEvent> pieceMovedHandler)
        {
            this.connection = connection;
            this.pieceMovedHandler = pieceMovedHandler;
            blockingCollection = new BlockingCollection<PieceMovedEvent>(new ConcurrentQueue<PieceMovedEvent>());

            blockingCollection.Add(new PieceMovedEvent());

            Task.Factory.StartNew(ConsumingTask, TaskCreationOptions.LongRunning);

            connection.On<PieceMovedEvent>(nameof(IMoveHubClient.OnPieceMoved), OnPieceMoved);
        }

        private void OnPieceMoved(PieceMovedEvent @event)
        {
            pieceMovedHandler(@event);

            //blockingCollection.TryAdd(@event);
            //var c = blockingCollection.Count;
        }

        private void ConsumingTask()
        {
            foreach (var @event in blockingCollection.GetConsumingEnumerable())
            {
                pieceMovedHandler(@event);
            }
        }

        public async ValueTask DisposeAsync()
        {
            blockingCollection.Dispose();
            await connection.DisposeAsync();
        }
    }
}
