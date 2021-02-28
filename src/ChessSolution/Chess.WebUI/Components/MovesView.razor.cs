using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Chess.WebUI.Components
{
    public partial class MovesView
    {
        [Parameter]
        public BoardViewModel BoardViewModel { get; set; }
    }
}
