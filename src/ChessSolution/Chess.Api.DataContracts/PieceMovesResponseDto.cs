namespace Chess.Api.DataContracts
{
    public class PieceMovesResponseDto
    {
        public string GameId { get; set; }

        public PieceMoveDto[] Moves { get; set; }
    }
}
