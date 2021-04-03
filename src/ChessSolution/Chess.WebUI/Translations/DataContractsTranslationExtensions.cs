using Chess.Api.DataContracts;
using Chess.Domain.Movement;
using Chess.Domain;
using System;

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

        private static Location AsDomain(this LocationDto location)
        {
            return new Location(location.Column, location.Row);
        }

        public static GameResult AsDomain(this Messages.GameResult gameResult)
        {
            return gameResult switch
            {
                Messages.GameResult.WonByBlack => GameResult.WonByBlack,
                Messages.GameResult.WonByWhite => GameResult.WonByWhite,
                Messages.GameResult.Draw => GameResult.Draw,

                _ => throw new ArgumentException(),
            };
        }
    }
}
