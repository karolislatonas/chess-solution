namespace Chess.Api.DataContracts
{
    public class PieceMoveResponseDto
    {
        public string GameId { get; set; }

        public PieceMoveDto Move { get; set; }
    }
}
