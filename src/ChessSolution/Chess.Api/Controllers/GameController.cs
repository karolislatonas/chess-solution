using Chess.Api.DataContracts;
using Chess.Api.Translators;
using Chess.Data;
using Chess.Messages.Commands;
using Chess.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Chess.Api.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IGamesRepository gameRepository;

        public GameController(IGamesRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        [HttpGet("{gameId}")]
        public IActionResult GetGame(string gameId)
        {
            return Ok(FindGame(gameId));
        }

        [HttpPost()]
        public IActionResult StartGame()
        {
            var startGameCommandHandler = new StartGameCommanHandler(gameRepository);

            var newGameId = startGameCommandHandler.ExecuteCommand(new StartNewGameCommand());

            return CreatedAtAction(
                nameof(GetGame),
                new { gameId = newGameId },
                FindGame(newGameId));
        }

        private GameDto FindGame(string gameId)
        {
            return gameRepository.GetGame(gameId).AsDataContract();
        }

    }
}
