using Microsoft.AspNetCore.Mvc;

namespace Chess.Api.Controllers
{
    [Route("game/{gameId}")]
    public class MoveController : ControllerBase
    {
        [HttpGet("moves")]
        public void GetGameMoves(int gameId)
        {

        }

        [HttpPost("move")]
        public void MovePiece(int gameId)
        {

        }

    }
}
