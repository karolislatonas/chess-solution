using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components;
using System;

namespace Chess.WebUI.Pages
{
    public partial class GameView : IDisposable
    {
        [Parameter]
        public string GameId { get; set; }

        [Inject]
        public BoardViewModel BoardViewModel { get; set; }

        protected override void OnInitialized()
        {
            BoardViewModel.OnStateChanged += StateHasChanged;
        }

        public void Dispose()
        {
            BoardViewModel.OnStateChanged -= StateHasChanged;
        }
    }
}
