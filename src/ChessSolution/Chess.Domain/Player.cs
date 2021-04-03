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

        public GameResult GetResignResult()
        {
            switch (PlaysAs)
            {
                case ChessColor.White:
                    return GameResult.WonByBlack;

                case ChessColor.Black:
                    return GameResult.WonByWhite;

                default:
                    throw new Exception();
            }
        }

        private void EnsurePlayerIdIsValid(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
                throw new ArgumentException("Player id cannot be empty");
        }

        public static Player AsWhite(string id) => new Player(id, ChessColor.White);

        public static Player AsBlack(string id) => new Player(id, ChessColor.Black);
    }
}
