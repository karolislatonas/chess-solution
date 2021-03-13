using System;

namespace Chess.Messaging
{
    public interface IServiceBus
    {
        void Subscribe<T>(object receiver, Action<T> messageHandler);

        void Unsubscribe<T>(object receiver, Action<T> messageHandler);

        void Publish<T>(T message);
    }
}
