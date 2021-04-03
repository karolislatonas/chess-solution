using Chess.Data;
using Chess.Domain;
using Chess.Messages.Commands;
using System;

namespace Chess.UseCases
{
    public class StartNewGameCommandHandler : CommandHandlerBase<StartNewGameCommand, string>
    {
        private readonly IGamesRepository gameRepository;

        public StartNewGameCommandHandler(IGamesRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public override string ExecuteCommand(StartNewGameCommand command)
        {
            var newGameId = Guid.NewGuid().ToString();

            var newGame = new Game(newGameId, command.WhitePlayerId, command.BlackPlayerId);

            gameRepository.AddGame(newGame);

            return newGame.GameId;
        }
    }
}
