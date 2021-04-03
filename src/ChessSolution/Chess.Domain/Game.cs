using System;

namespace Chess.Domain
{
    public class Game
    {
        public Game(string id, string whitePlayerId, string blackPlayerId)
        {
            GameId = id;
            WhitePlayerId = whitePlayerId;
            BlackPlayerId = blackPlayerId;
        }

        public string GameId { get; }

        public GameResult? Result { get; private set; }

        public bool IsFinished => Result.HasValue;

        public string WhitePlayerId { get; }

        public string BlackPlayerId { get; }

        public void ResignByPlayer(string playerId)
        {
            var resignedBy = GetPlayerColor(playerId);

            UpdateGameResult(resignedBy);
        }

        public void MakeDraw()
        {
            Result = GameResult.Draw;
        }

        private ChessColor GetPlayerColor(string playerId)
        {
            if (playerId == WhitePlayerId)
                return ChessColor.White;

            if (playerId == BlackPlayerId)
                return ChessColor.Black;

            throw new Exception($"Missing player {playerId ?? "NULL"} in game {GameId}");
        }

        private void UpdateGameResult(ChessColor resignedBy)
        {
            switch (resignedBy)
            {
                case ChessColor.White:
                    Result = GameResult.WonByBlack;
                    return;

                case ChessColor.Black:
                    Result = GameResult.WonByWhite;
                    return;

                default:
                    throw new Exception();
            }
        }
    }
}
