using Chess.Api.DataContracts;
using Chess.Shared.DataContracts.Translations;
using Chess.Domain;

namespace Chess.Api.Translators
{
    public static class DomainTransalationExtensions
    {
        public static GameResponseDto AsDto(this Game game)
        {
            return new GameResponseDto
            {
                GameId = game.GameId,
                WhitePlayerId = game.WhitePlayerId,
                BlackPlayerId = game.BlackPlayerId,
                Result = game.Result.AsDto()
            };
        }
    }
}
