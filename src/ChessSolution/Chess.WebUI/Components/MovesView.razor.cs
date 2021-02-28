using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Chess.WebUI.Components
{
    public partial class MovesView
    {
        [Parameter]
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
