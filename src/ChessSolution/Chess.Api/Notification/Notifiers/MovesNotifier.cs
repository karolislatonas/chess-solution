using Chess.Messaging;
using Chess.Messages.Events;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Chess.SignalR.Typings;

namespace Chess.Api.Notification.Notifiers
{
    public class MovesNotifier : INotifier
    {
        private readonly string gameId;
        private readonly IServiceBus serviceBus;
        private readonly IClientProxy clientProxy;

        public MovesNotifier(string gameId, IServiceBus serviceBus, IClientProxy clientProxy)
        {
            this.gameId = gameId;
            this.serviceBus = serviceBus;
            this.clientProxy = clientProxy;
        }

        Task INotifier.StartAsync()
        {
            Start();

            return Task.CompletedTask;
        }

        public void Start()
        {
            serviceBus.Subscribe<PieceMovedEvent>(this, OnPieceMoved);
        }

        public void Dispose()
        {
            serviceBus.Unsubscribe<PieceMovedEvent>(this, OnPieceMoved);
        }

        private async void OnPieceMoved(PieceMovedEvent pieceMovedEvent)
        {
            if (pieceMovedEvent.GameId != gameId)
                return;

            await clientProxy.SendAsync(nameof(IMoveHubClient.OnPieceMoved), pieceMovedEvent);
        }
    }
}
