using Chess.Domain;
using Microsoft.AspNetCore.Components;

namespace Chess.WebUI.Components
{
    public partial class GameResultView
    {
        [Parameter]
        public GameResult GameResult { get; set; }

        public bool WasClosed { get; private set; }

        public void Close()
        {
            WasClosed = true;
        }
    }
}
