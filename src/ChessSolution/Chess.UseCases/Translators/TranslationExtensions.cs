using Chess.Domain.Movement;
using Chess.Messages;

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
    }
}
