using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Chess.WebUI.Pages
{
    public partial class IndexView
    {
        [Inject]
        public IndexViewModel IndexViewModel { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public async Task OnStartNewGameButtonClickedAsync()
        {
            var newGameId = await IndexViewModel.StartNewGameAsync();

            NavigationManager.NavigateTo($"game/{newGameId}");
        }
    }
}
