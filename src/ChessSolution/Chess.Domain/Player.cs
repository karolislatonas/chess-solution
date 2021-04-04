using Chess.Domain.Extensions;
using System;

namespace Chess.Domain
{
    public class Player
    {
        private Player(string id, ChessColor playsAs)
        {
            EnsurePlayerIdIsValid(id);

            Id = id;
            PlaysAs = playsAs;
        }

        public string Id { get; }

        public ChessColor PlaysAs { get; }

        public ChessColor OpponentColor() => PlaysAs.GetOpposite();

        public static Player AsWhite(string id) => new Player(id, ChessColor.White);

        public static Player AsBlack(string id) => new Player(id, ChessColor.Black);

        private void EnsurePlayerIdIsValid(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
                throw new ArgumentException("Player id cannot be empty");
        }
    }
}
