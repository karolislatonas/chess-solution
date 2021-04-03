namespace Chess.Messages.Commands
{
    public class ResignGameCommand
    {
        public string GameId { get; set; }

        public string PlayerId { get; set; }
    }
}
