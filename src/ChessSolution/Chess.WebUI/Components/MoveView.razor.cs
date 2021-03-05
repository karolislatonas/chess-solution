using Chess.Domain;
using Chess.Domain.Movement;
using Microsoft.AspNetCore.Components;
using System;

namespace Chess.WebUI.Components
{
    public partial class MoveView
    {
        [Parameter]
        public PieceMove Move { get; set; }

        [Parameter]
        public BoardDetails BoardDetails { get; set; }

        [Parameter]
        public Action OnSelect { get; set; }

        [Parameter]
        public bool IsSelected { get; set; }
    }
}
