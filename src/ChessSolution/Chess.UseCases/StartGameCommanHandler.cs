using Chess.Data;
using Chess.Domain;
using Chess.Messages.Commands;
using System;

namespace Chess.UseCases
{
    public class StartGameCommanHandler : CommandHandlerBase<StartNewGameCommand, string>
    {
        private readonly IGameRepository gameRepository;

        public StartGameCommanHandler(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public override string ExecuteCommand(StartNewGameCommand command)
        {
            var newGameId = Guid.NewGuid().ToString();

            var newGame = new Game(newGameId);

            gameRepository.AddGame(newGame);

            return newGame.GameId;
        }
    }
}
