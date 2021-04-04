using Chess.Domain.Movement;
using System;
using Chess.Shared.DataContracts;
using Chess.Domain;

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

        public static Location AsDomain(this LocationDto location)
        {
            return new Location(location.Column, location.Row);
        }

        public static GameResult AsDomain(this GameResultDto gameResult)
        {
            return gameResult switch
            {
                GameResultDto.WonByBlack => GameResult.WonByBlack,
                GameResultDto.WonByWhite => GameResult.WonByWhite,
                GameResultDto.Draw => GameResult.Draw,

                _ => throw new ArgumentException("Failed to translate game result dto to domain"),
            };
        }
    }
}
