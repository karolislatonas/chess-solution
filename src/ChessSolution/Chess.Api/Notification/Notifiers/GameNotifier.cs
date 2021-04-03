using Chess.Messages.Events;
using Chess.Messaging;
using Chess.SignalR.Typings;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Chess.Api.Notification.Notifiers
{
    public class GameNotifier : INotifier
    {
        private readonly string gameId;
        private readonly IServiceBus serviceBus;
        private readonly IClientProxy clientProxy;

        public GameNotifier(string gameId, IServiceBus serviceBus, IClientProxy clientProxy)
        {
            this.gameId = gameId;
            this.serviceBus = serviceBus;
            this.clientProxy = clientProxy;
        }

        public async Task StartAsync()
        {
            serviceBus.Subscribe<GameFinishedEvent>(this, OnGameFinished);

            await Task.CompletedTask;
        }

        private async void OnGameFinished(GameFinishedEvent @event)
        {
            if (@event.GameId != gameId)
                return;

            await clientProxy.SendAsync(nameof(IGameHubClient.OnGameFinished), @event);
        }

        public void Dispose()
        {
            serviceBus.Unsubscribe<GameFinishedEvent>(this, OnGameFinished);
        }
    }
}
