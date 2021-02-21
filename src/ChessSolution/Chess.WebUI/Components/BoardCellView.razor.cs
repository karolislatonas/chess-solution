using Chess.Domain.Pieces;
using Microsoft.AspNetCore.Components;

namespace Chess.WebUI.Components
{
    public partial class BoardCellView
    {
        [Parameter]
        public int Row { get; set; }

        [Parameter]
        public int Column { get; set; }

        [Parameter]
        public IPiece Piece { get; set; }
    }
}
