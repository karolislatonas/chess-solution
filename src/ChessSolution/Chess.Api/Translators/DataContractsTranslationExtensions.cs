using Chess.Api.DataContracts;
using Chess.Messages.Commands;

namespace Chess.Api.Translators
{
    public static class DataContractsTranslationExtensions
    {
        public static MovePieceCommand AsCommand(this MovePieceRequestDto movePieceDto, string gameId)
        {
            return new MovePieceCommand
            {
                GameId = gameId,
                From = movePieceDto.From.AsCommandModel(),
                To = movePieceDto.To.AsCommandModel()
            };
        }

        private static Messages.LocationDto AsCommandModel(this LocationDto location)
        {
            return new Messages.LocationDto
            {
                Column = location.Column,
                Row = location.Row
            };
        }
    }
}
