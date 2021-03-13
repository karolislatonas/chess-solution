using System;

namespace Chess.Messaging
{
    public class InMemoryServiceBus : IServiceBus
    {
        private readonly PubSub.Hub hub;

        public InMemoryServiceBus()
        {
            hub = PubSub.Hub.Default;
        }

        public void Publish<T>(T message)
        {
            hub.Publish(message);
        }

        public void Subscribe<T>(object receiver, Action<T> messageHandler)
        {
            hub.Subscribe(messageHandler);
        }

        public void Unsubscribe<T>(object receiver, Action<T> messageHandler)
        {
            hub.Unsubscribe(receiver, messageHandler);
        }
    }
}
