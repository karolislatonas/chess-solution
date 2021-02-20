using Microsoft.AspNetCore.Mvc;

namespace Chess.Api.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        [HttpPost()]
        public void StartGame()
        {
             
        }

    }
}
