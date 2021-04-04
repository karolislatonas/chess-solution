using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Shared.DataContracts;
using System;

namespace Chess.UseCases.Translators
{
    public static class TranslationExtensions
    {
        public static Location AsDomain(this LocationDto locationDto)
        {
            return new Location(locationDto.Column, locationDto.Row);
        }

        public static LocationDto AsMessage(this Location location)
        {
            return new LocationDto
            {
                Column = location.Column,
                Row = location.Row
            };
        }

        public static GameResultDto AsMessage(this GameResult gameResult)
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
