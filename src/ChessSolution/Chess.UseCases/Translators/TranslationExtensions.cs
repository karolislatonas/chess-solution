using Chess.Domain.Movement;
using Chess.Messages;
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

        public static GameResult AsMessage(this Domain.GameResult gameResult)
        {
            return gameResult switch
            {
                Domain.GameResult.Draw => GameResult.Draw,
                Domain.GameResult.WonByBlack => GameResult.WonByBlack,
                Domain.GameResult.WonByWhite => GameResult.WonByWhite,

                _ => throw new Exception()
            };
        }
    }
}
