using Chess.Api.Client.Subscription.Subscribers;
using Chess.Messages.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chess.Api.Client.Subscription
{
    public interface ISubscriptionProvider
    {
        Task<ISubscriber> SubscribeAsync(string gameId, Action<PieceMovedEvent> handler);

        Task<ISubscriber> SubscribeAsync(string gameId, int fromSequenceNumber, Action<PieceMovedEvent> handler);
    }
}
