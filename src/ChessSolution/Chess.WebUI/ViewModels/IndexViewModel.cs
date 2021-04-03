using Chess.Api.Client;
using System;
using System.Threading.Tasks;

namespace Chess.WebUI.ViewModels
{
    public class IndexViewModel
    {
        private readonly GameService gameService;

        public IndexViewModel(GameService gameService)
        {
            this.gameService = gameService;
        }

        public async Task<string> StartNewGameAsync()
        {
            var whitePlayerId = Guid.NewGuid().ToString();
            var blackPlayerId = Guid.NewGuid().ToString();

            return await gameService.StartNewGameAsync(whitePlayerId, blackPlayerId);
        }
    }
}
