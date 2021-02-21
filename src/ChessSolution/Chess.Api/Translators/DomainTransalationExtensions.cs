using Chess.Api.DataContracts;
using Chess.Domain;
using Chess.Domain.Movement;

namespace Chess.Api.Translators
{
    public static class DomainTransalationExtensions
    {
        public static PieceMoveDto AsDataContract(this PieceMove pieceMove)
        {
            return new PieceMoveDto
            {
                From = pieceMove.From.AsDataContract(),
                To = pieceMove.To.AsDataContract(),
                SequenceNumber = pieceMove.SequenceNumber
            };
        }

        public static GameDto AsDataContract(this Game game)
        {
            return new GameDto
            {
                GameId = game.GameId
            };
        }

        private static LocationDto AsDataContract(this Location location)
        {
            return new LocationDto
            {
                Column = location.Column,
                Row = location.Row
            };
        }
    }
}
