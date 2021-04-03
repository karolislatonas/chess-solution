using System;

namespace Chess.Domain
{
    public class Game
    {
        private readonly Player whitePlayer;
        private readonly Player blackPlayer;

        public Game(string id, string whitePlayerId, string blackPlayerId)
        {
            GameId = id;

            whitePlayer = Player.AsWhite(whitePlayerId);
            blackPlayer = Player.AsBlack(blackPlayerId);
        }

        public string GameId { get; }

        public GameResult? Result { get; private set; }

        public bool IsFinished => Result.HasValue;

        public string WhitePlayerId => whitePlayer.Id;

        public string BlackPlayerId => blackPlayer.Id;

        public void ResignByPlayer(string playerId)
        {
            var resignedByPlayer = GetPlayerById(playerId);

            Result = resignedByPlayer.GetResignResult();
        }

        public void MakeDraw()
        {
            Result = GameResult.Draw;
        }

        private Player GetPlayerById(string playerId)
        {
            if (playerId == whitePlayer.Id)
                return whitePlayer;

            if (playerId == blackPlayer.Id)
                return blackPlayer;

            throw new Exception($"Missing player {playerId ?? "NULL"} in game {GameId}");
        }
    }
}
