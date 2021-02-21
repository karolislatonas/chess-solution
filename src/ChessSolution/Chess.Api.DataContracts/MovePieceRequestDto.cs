namespace Chess.Api.DataContracts
{
    public class MovePieceRequestDto
    {
        public LocationDto From { get; set; }

        public LocationDto To { get; set; }
    }
}
