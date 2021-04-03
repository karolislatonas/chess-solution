﻿using Chess.Data;
using Chess.Messages;
using Chess.Messages.Events;
using Chess.Messaging;
using Chess.SignalR.Typings;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Api.Notification.Notifiers
{
    public class MovesNotifier : INotifier
    {
        private readonly string gameId;
        private readonly int fromSequenceNumber;
        private readonly IServiceBus serviceBus;
        private readonly IClientProxy clientProxy;
        private readonly IMovesRepository movesRepository;

        public MovesNotifier(string gameId, int fromSequenceNumber, IServiceBus serviceBus, IMovesRepository movesRepository, IClientProxy clientProxy)
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
            
            await PublishReceivedEvents(queue, lastSequenceNumberPublished);

            serviceBus.Subscribe<PieceMovedEvent>(this, OnPieceMoved);
            serviceBus.Unsubscribe<PieceMovedEvent>(this, queue.Enqueue);
        }

        private async void OnPieceMoved(PieceMovedEvent pieceMovedEvent)
        {
            await OnPieceMovedAsync(pieceMovedEvent);
        }

        private async Task OnPieceMovedAsync(PieceMovedEvent pieceMovedEvent)
        {
            if (pieceMovedEvent.GameId != gameId)
                return;

            await clientProxy.SendAsync(nameof(IGameHubClient.OnPieceMoved), pieceMovedEvent);
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

        private async Task PublishReceivedEvents(ConcurrentQueue<PieceMovedEvent> queue, int? fromSequence)
        {
            var lastSequenceNumberPublished = fromSequence;

            while (queue.TryDequeue(out var movedEvent))
            {
                if (movedEvent.SequenceNumber <= lastSequenceNumberPublished)
                    continue;

                await OnPieceMovedAsync(movedEvent);
                lastSequenceNumberPublished = movedEvent.SequenceNumber;
            }
        }

        public void Dispose()
        {
            serviceBus.Unsubscribe<PieceMovedEvent>(this, OnPieceMoved);
        }
    }
}
