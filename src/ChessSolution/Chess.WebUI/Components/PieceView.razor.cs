using Chess.Domain.Pieces;
using Microsoft.AspNetCore.Components;
using System;

namespace Chess.WebUI.Components
{
    public partial class PieceView
    {
        [Parameter]
        public int Row { get; set; }

        [Parameter]
        public int Column { get; set; }

        [Parameter]
        public IPiece Piece { get; set; }

        [Parameter]
        public bool IsDragged { get; set; }

        [Parameter]
        public Action<PieceView> OnDragStarted { get; set; }

        [Parameter]
        public Action<PieceView> OnDragEnded { get; set; }

        private void HandleDragStarted()
        {
            IsDragged = true;
            OnDragStarted?.Invoke(this);
        }

        private void HandleDragEnded()
        {
            IsDragged = false;
            OnDragEnded?.Invoke(this);
        }
    }
}
