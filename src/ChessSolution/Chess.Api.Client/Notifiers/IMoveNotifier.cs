using Chess.Api.DataContracts;
using Chess.Messages.Events;
using System;
using System.Threading.Tasks;

namespace Chess.Api.Client.Notifiers
{
    public interface IMoveNotifier : IDisposable
    {
        Task SubscribeAsync(string gameId, Action<PieceMovedEvent> handler);

        Task SubscribeFromStartAsync(string gameId, Action<PieceMovedEvent> handler);
    }
}
