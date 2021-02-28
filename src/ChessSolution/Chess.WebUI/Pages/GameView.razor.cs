﻿using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Chess.WebUI.Pages
{
    public partial class GameView
    {
        [Parameter]
        public string GameId { get; set; }

        [Inject]
        public BoardViewModel BoardViewModel { get; set; }
    }
}
