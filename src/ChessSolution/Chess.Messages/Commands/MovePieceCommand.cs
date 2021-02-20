namespace Chess.Messages.Commands
{
    public class MovePieceCommand
    {
        public string GameId { get; set; }

        public LocationDto From { get; set; }

        public LocationDto To { get; set; }
    }
}
