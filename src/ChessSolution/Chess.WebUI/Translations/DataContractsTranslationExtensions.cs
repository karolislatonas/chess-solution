using Chess.Api.DataContracts;
using Chess.Domain.Movement;

namespace Chess.WebUI.Translations
{
    public static class DataContractsTranslationExtensions
    {
        public static PieceMove AsDomain(this PieceMoveDto pieceMove)
        {
            return new PieceMove(
                pieceMove.SequenceNumber,
                pieceMove.From.AsDomain(),
                pieceMove.To.AsDomain());
        }

        private static Location AsDomain(this LocationDto location)
        {
            return new Location(location.X, location.Y);
        }
    }
}
