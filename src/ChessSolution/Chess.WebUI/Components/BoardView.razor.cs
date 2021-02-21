using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Chess.WebUI.Components
{
    public partial class BoardView
    {
        [Parameter]
        public string GameId { get; set; }

        [Inject]
        public BoardViewModel GameViewModel { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GameViewModel.InitialiseAsync(GameId);
        }
    }
}
