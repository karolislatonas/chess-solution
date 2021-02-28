using Chess.Domain.Movement;
using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Chess.WebUI.Components
{
    public partial class BoardView
    {
        [Parameter]
        public string GameId { get; set; }

        [Parameter]
        public BoardViewModel BoardViewModel { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await BoardViewModel.InitialiseAsync(GameId);
        }

        public void OnPieceDragStarted(PieceView piece)
        {
            BoardViewModel.SelectPieceAt(piece.Column, piece.Row);
            StateHasChanged();
        }

        public void OnPieceDragEnded(PieceView piece)
        {
            BoardViewModel.ClearPieceSelection();
        }

        public async Task OnDropAsync(Location location)
        {
            await BoardViewModel.MoveSelectedPieceToAsync(location);
        }
    }
}
