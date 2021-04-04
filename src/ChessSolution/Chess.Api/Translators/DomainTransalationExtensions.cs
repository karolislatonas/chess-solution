using Chess.Api.DataContracts;
using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Shared.DataContracts;

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

        public static GameResponseDto AsDataContract(this Game game)
        {
            return new GameResponseDto
            {
                GameId = game.GameId,
                WhitePlayerId = game.WhitePlayerId,
                BlackPlayerId = game.BlackPlayerId
            };
        }

        public static LocationDto AsDataContract(this Location location)
        {
            return new LocationDto
            {
                Column = location.Column,
                Row = location.Row
            };
        }
    }
}
