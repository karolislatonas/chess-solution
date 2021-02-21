using Chess.Domain.Movement;
using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Chess.WebUI.Components
{
    public partial class BoardView
    {
        private PieceView draggedPiece;

        [Parameter]
        public string GameId { get; set; }

        [Inject]
        public BoardViewModel BoardViewModel { get; set; }


        protected async override Task OnInitializedAsync()
        {
            await BoardViewModel.InitialiseAsync(GameId);
        }

        public void OnPieceDragStarted(PieceView piece)
        {
            draggedPiece = piece;
        }

        public void OnPieceDragEnded(PieceView piece)
        {
            draggedPiece = null;
        }

        public async Task OnDropAsync(Location location)
        {
            if (draggedPiece == null)
                return;

            await BoardViewModel.MovePieceAsync(
                new Location(draggedPiece.Column, draggedPiece.Row),
                new Location(location.Column, location.Row));

            draggedPiece = null;
        }
    }
}
