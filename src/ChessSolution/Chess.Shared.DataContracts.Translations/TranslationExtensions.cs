using Chess.Domain;
using Chess.Domain.Movement;
using System;

namespace Chess.Shared.DataContracts.Translations
{
    public static class TranslationExtensions
    {
        public static PieceMove AsDomain(this PieceMoveDto pieceMove)
        {
            return new PieceMove(
                pieceMove.SequenceNumber,
                pieceMove.From.AsDomain(),
                pieceMove.To.AsDomain());
        }

        public static Location AsDomain(this LocationDto locationDto)
        {
            return new Location(locationDto.Column, locationDto.Row);
        }

        public static GameResult? AsDomain(this GameResultDto? gameResult)
        {
            if (!gameResult.HasValue)
                return null;

            return gameResult.Value.AsDomain();
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

        public static PieceMoveDto AsDto(this PieceMove pieceMove)
        {
            return new PieceMoveDto
            {
                From = pieceMove.From.AsDto(),
                To = pieceMove.To.AsDto(),
                SequenceNumber = pieceMove.SequenceNumber
            };
        }

        public static LocationDto AsDto(this Location location)
        {
            return new LocationDto
            {
                Column = location.Column,
                Row = location.Row
            };
        }

        public static GameResultDto? AsDto(this GameResult? gameResult)
        {
            if (!gameResult.HasValue)
                return null;

            return gameResult.Value.AsDto();
        }

        public static GameResultDto AsDto(this GameResult gameResult)
        {
            return gameResult switch
            {
                GameResult.Draw => GameResultDto.Draw,
                GameResult.WonByBlack => GameResultDto.WonByBlack,
                GameResult.WonByWhite => GameResultDto.WonByWhite,

                _ => throw new Exception()
            };
        }


    }
}
