﻿using Chess.Data;
using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Messages.Commands;
using Chess.Messages.DomainTranslation;
using System;
using System.Linq;

namespace Chess.UseCases
{
    public class MovePieceCommandHandler : CommandHandlerBase<MovePieceCommand, int>
    {
        private readonly IMovesRepository movesRepository;

        public MovePieceCommandHandler(IMovesRepository movesRepository)
        {
            this.movesRepository = movesRepository;
        }

        public override int ExecuteCommand(MovePieceCommand command)
        {
            var movesLog = GetMovesLog(command.GameId);

            var board = CreateBoard(movesLog);

            var from = command.From.ToDomain();
            var to = command.To.ToDomain();

            EnsureIsPlayerTurn(board, movesLog, from);
            EnsureIsValidMove(board, movesLog, from, to);

            SaveLastMove(command.GameId, movesLog);

            return movesLog.NextMoveSequenceNumber();
        }

        private void SaveLastMove(string gameId, MovesLog movesLog)
        {
            var lastMove = movesLog.LastMove;

            var pieceMove = new PieceMove(lastMove.SequenceNumber, lastMove.Move.From, lastMove.Move.To);
            movesRepository.AddMove(gameId, pieceMove);
        }

        private void EnsureIsPlayerTurn(Board board, MovesLog movesLog, Location from)
        {
            var playerColor = board.GetPieceAt(from).Color;
            var turnsTracker = new TurnsTracker(movesLog);
            
            if(!turnsTracker.IsTurnFor(playerColor))
                throw new Exception("Wrong player moving");
        }

        private MovesLog GetMovesLog(string gameId)
        {
            var movesSequenceTranslator = new MoveSequenceTranslator();

            var allMoves = movesRepository
                .GetGameMoves(gameId)
                .Select(movesSequenceTranslator.TranslateNextMove);

            return new MovesLog(allMoves);
        }

        private Board CreateBoard(MovesLog movesLog)
        {
            var board = new Board();
            board.ApplyMoves(movesLog);

            return board;
        }

        private void EnsureIsValidMove(Board board, MovesLog movesLog, Location from, Location to)
        {
            var pieceMover = new PieceMover();

            pieceMover.EnsureIsValidMove(board, movesLog, from, to);
        }
    }
}
