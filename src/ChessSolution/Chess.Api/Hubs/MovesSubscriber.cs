using Chess.Messaging;
using Chess.Messages.Events;
using Microsoft.AspNetCore.SignalR;

namespace Chess.Api.Hubs
{
    public class MovesSubscriber
    {
        private readonly string gameId;
        private readonly IServiceBus serviceBus;
        private readonly IClientProxy clientProxy;

        public MovesSubscriber(string gameId, IServiceBus serviceBus, IClientProxy clientProxy)
        {
            this.gameId = gameId;
            this.serviceBus = serviceBus;
            this.clientProxy = clientProxy;
            
            serviceBus.Subscribe<PieceMovedEvent>(this, OnPieceMoved);
        }

        public void Unsubscribe()
        {
            serviceBus.Unsubscribe<PieceMovedEvent>(this, OnPieceMoved);
        }

        private async void OnPieceMoved(PieceMovedEvent pieceMovedEvent)
        {
            if (pieceMovedEvent.GameId != gameId)
                return;

            await clientProxy.SendAsync("OnPieceMoved", pieceMovedEvent);
        }
    }
}
