﻿using Chess.Api.DataContracts;
using Chess.Messaging;
using Chess.Api.Translators;
using Chess.Data;
using Chess.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Chess.Shared.DataContracts.Translations;

namespace Chess.Api.Controllers
{
    [Route("api/game/{gameId}/moves")]
    public class MoveController : ControllerBase
    {
        private readonly IGamesRepository gamesRepository;
        private readonly IMovesRepository moveRepository;
        private readonly IServiceBus serviceBus;

        public MoveController(IGamesRepository gamesRepository, IMovesRepository moveRepository, IServiceBus serviceBus)
        {
            this.gamesRepository = gamesRepository;
            this.moveRepository = moveRepository;
            this.serviceBus = serviceBus;
        }

        [HttpGet]
        public IActionResult GetGameMoves(string gameId)
        {
            var allGameMoves = moveRepository.GetGameMoves(gameId);

            var response = new PieceMovesResponseDto
            {
                GameId = gameId,
                Moves = allGameMoves.Select(m => m.AsDto()).ToArray()
            };

            return Ok(response);
        }

        [HttpGet("{sequenceNumber}")]
        public IActionResult GetMove(string gameId, int sequenceNumber)
        {
            return Ok(GetPieceMoveResponse(gameId, sequenceNumber));
        }

        [HttpPost]
        public IActionResult MovePiece(string gameId, [FromBody] MovePieceRequestDto movePieceRequest)
        {
            var command = movePieceRequest.AsCommand(gameId);

            var commandHandler = new MovePieceCommandHandler(gamesRepository, moveRepository, serviceBus);

            var moveSequenceNumber = commandHandler.ExecuteCommand(command);

            return CreatedAtAction(
                nameof(GetMove),
                new { gameId = gameId, sequenceNumber = moveSequenceNumber },
                GetPieceMoveResponse(gameId, moveSequenceNumber));
        }

        private PieceMoveResponseDto GetPieceMoveResponse(string gameId, int sequenceNumber)
        {
            var move = moveRepository.GetMove(gameId, sequenceNumber);

            return new PieceMoveResponseDto
            {
                GameId = gameId,
                Move = move.AsDto()
            };
        }

    }
}
