using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Chess.WebUI.Pages
{
    public partial class GameView
    {
        [Parameter]
        public string GameId { get; set; }

        
    }
}
