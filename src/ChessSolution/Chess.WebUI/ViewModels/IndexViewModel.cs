using Chess.Api.Client;
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
            return await gameService.StartNewGameAsync();
        }
    }
}
