using Chess.Data;
using Chess.Domain;
using Chess.Messages.Commands;
using Chess.Messages.Events;
using Chess.Messaging;
using Chess.UseCases.Translators;
using System;

namespace Chess.UseCases
{
    public class ResignGameCommandHandler : CommandHandlerBase<ResignGameCommand>
    {
        private readonly IGamesRepository gamesRepository;
        private readonly IServiceBus serviceBus;

        public ResignGameCommandHandler(IGamesRepository gamesRepository, IServiceBus serviceBus)
        {
            this.gamesRepository = gamesRepository;
            this.serviceBus = serviceBus;
        }

        public override void ExecuteCommand(ResignGameCommand command)
        {
            EnsureCommandIsValid(command);

            var game = gamesRepository.GetGame(command.GameId);

            EnsureGameNotFinished(game);

            game.ResignByPlayer(command.PlayerId);

            gamesRepository.UpdateGame(game);

            RaiseGameFinishedEvent(game);
        }

        private void RaiseGameFinishedEvent(Game game)
        {
            var gameFinishedEvent = new GameFinishedEvent
            {
                GameId = game.GameId,
                Result = game.Result.Value.AsMessage()
            };

            serviceBus.Publish(gameFinishedEvent);
        }

        private void EnsureCommandIsValid(ResignGameCommand command)
        {
            if (string.IsNullOrEmpty(command.GameId))
                throw new Exception("Missing game id for player resign");

            if (string.IsNullOrEmpty(command.PlayerId))
                throw new Exception("Missing resigning player id");
        }

        private void EnsureGameNotFinished(Game game)
        {
            if (game.IsFinished)
                throw new Exception("Cannot move pieces in finished game.");
        }
    }
}
