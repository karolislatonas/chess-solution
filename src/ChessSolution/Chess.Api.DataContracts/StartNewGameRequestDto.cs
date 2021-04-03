namespace Chess.Api.DataContracts
{
    public class StartNewGameRequestDto
    {
        public string WhitePlayerId { get; set; }

        public string BlackPlayerId { get; set; }
    }
}
