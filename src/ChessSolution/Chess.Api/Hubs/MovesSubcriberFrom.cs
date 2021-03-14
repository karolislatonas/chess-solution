using Chess.Data;
using Chess.Messages;
using Chess.Messages.Events;
using Chess.Messaging;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Api.Hubs
{
    public class MovesSubcriberFrom : ISubscriber
    {
        private readonly string gameId;
        private readonly int fromSequenceNumber;
        private readonly IServiceBus serviceBus;
        private readonly IClientProxy clientProxy;
        private readonly IMovesRepository movesRepository;

        public MovesSubcriberFrom(string gameId, int fromSequenceNumber, IServiceBus serviceBus, IMovesRepository movesRepository, IClientProxy clientProxy)
        {
            this.gameId = gameId;
            this.fromSequenceNumber = fromSequenceNumber;
            this.serviceBus = serviceBus;
            this.clientProxy = clientProxy;
            this.movesRepository = movesRepository;
        }

        public async Task StartAsync()
        {
            var queue = new ConcurrentQueue<PieceMovedEvent>();
            serviceBus.Subscribe<PieceMovedEvent>(this, queue.Enqueue);

            var lastSequenceNumberPublished = await PublishSavedEvents();

            while (queue.TryDequeue(out var movedEvent))
            {
                if (movedEvent.SequenceNumber <= lastSequenceNumberPublished)
                    continue;

                await OnPieceMovedAsync(movedEvent);
                lastSequenceNumberPublished = movedEvent.SequenceNumber;
            }

            serviceBus.Subscribe<PieceMovedEvent>(this, OnPieceMoved);
            serviceBus.Unsubscribe<PieceMovedEvent>(this, queue.Enqueue);
            
        }

        public void Unsubscribe()
        {
            serviceBus.Unsubscribe<PieceMovedEvent>(this, OnPieceMoved);
        }

        private async void OnPieceMoved(PieceMovedEvent pieceMovedEvent)
        {
            await OnPieceMovedAsync(pieceMovedEvent);
        }

        private async Task OnPieceMovedAsync(PieceMovedEvent pieceMovedEvent)
        {
            if (pieceMovedEvent.GameId != gameId)
                return;

            await clientProxy.SendAsync("OnPieceMoved", pieceMovedEvent);
        }

        private async Task<int?> PublishSavedEvents()
        {
            var movedEvents = movesRepository
                .GetMovesFromSequence(gameId, fromSequenceNumber)
                .Select(m => new PieceMovedEvent
                {
                    GameId = gameId,
                    SequenceNumber = m.SequenceNumber,
                    From = new LocationDto { Column = m.From.Column, Row = m.From.Row },
                    To = new LocationDto { Column = m.To.Column, Row = m.To.Row }
                });

            int? lastSequenceNumberPublished = null;

            foreach (var movedEvent in movedEvents)
            {
                await OnPieceMovedAsync(movedEvent);
                lastSequenceNumberPublished = movedEvent.SequenceNumber;
            }

            return lastSequenceNumberPublished;
        }
    }
}
