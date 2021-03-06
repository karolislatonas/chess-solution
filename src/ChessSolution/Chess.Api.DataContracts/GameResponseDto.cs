﻿using Chess.Shared.DataContracts;

namespace Chess.Api.DataContracts
{
    public class GameResponseDto
    {
        public string GameId { get; set; }

        public string WhitePlayerId { get; set; }

        public string BlackPlayerId { get; set; }

        public GameResultDto? Result { get; set; }
    }
}
