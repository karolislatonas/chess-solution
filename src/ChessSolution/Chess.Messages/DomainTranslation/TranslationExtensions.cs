using Chess.Domain.Movement;

namespace Chess.Messages.DomainTranslation
{
    public static class TranslationExtensions
    {
        public static Location ToDomain(this LocationDto locationDto)
        {
            return new Location(locationDto.X, locationDto.Y);
        }
    }
}
