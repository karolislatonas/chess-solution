using Chess.Data;
using Chess.Domain;
using Chess.Messages.Commands;
using System;

namespace Chess.UseCases
{
    public class StartGameCommanHandler : CommandHandlerBase<StartNewGameCommand>
    {
        private readonly IGameRepository gameRepository;

        public StartGameCommanHandler(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public override void ExecuteCommand(StartNewGameCommand command)
        {
            var newGameId = Guid.NewGuid().ToString();

            var newGame = new Game(newGameId);

            gameRepository.AddGame(newGame);
        }
    }
}
