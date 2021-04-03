using Chess.Api.DataContracts;
using Chess.Api.Translators;
using Chess.Data;
using Chess.Messages.Commands;
using Chess.Messaging;
using Chess.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Chess.Api.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IGamesRepository gameRepository;
        private readonly IServiceBus serviceBus;

        public GameController(IGamesRepository gameRepository, IServiceBus serviceBus)
        {
            this.gameRepository = gameRepository;
            this.serviceBus = serviceBus;
        }

        [HttpGet("{gameId}")]
        public IActionResult GetGame(string gameId)
        {
            return Ok(FindGame(gameId));
        }

        [HttpPost()]
        public IActionResult StartGame(StartNewGameRequestDto startNewGameRequest)
        {
            var startNewGameCommandHandler = new StartNewGameCommandHandler(gameRepository);

            var startNewGameCommand = new StartNewGameCommand()
            {
                BlackPlayerId = startNewGameRequest.BlakcPlayerId,
                WhitePlayerId = startNewGameRequest.WhitePlayerId
            };

            var newGameId = startNewGameCommandHandler.ExecuteCommand(new StartNewGameCommand());

            return CreatedAtAction(
                nameof(GetGame),
                new { gameId = newGameId },
                FindGame(newGameId));
        }

        [HttpPost("{gameId}/resign")]
        public IActionResult Resign(string gameId, [FromQuery] string playerId)
        {
            var resignGameCommandHandler = new ResignGameCommandHandler(gameRepository, serviceBus);

            var command = new ResignGameCommand
            {
                GameId = gameId,
                PlayerId = playerId
            };

            resignGameCommandHandler.ExecuteCommand(command);

            return NoContent();
        }

        private GameResponseDto FindGame(string gameId)
        {
            return gameRepository.GetGame(gameId).AsDataContract();
        }

    }
}
