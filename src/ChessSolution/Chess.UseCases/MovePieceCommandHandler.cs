using Chess.Data;
using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Messages.Commands;
using Chess.Messages.Events;
using Chess.Messaging;
using Chess.Shared.DataContracts;
using Chess.Shared.DataContracts.Translations;
using System;
using System.Linq;

namespace Chess.UseCases
{
    public class MovePieceCommandHandler : CommandHandlerBase<MovePieceCommand, int>
    {
        private readonly IGamesRepository gamesRepository;
        private readonly IMovesRepository movesRepository;
        private readonly IServiceBus serviceBus;

        public MovePieceCommandHandler(IGamesRepository gamesRepository, IMovesRepository movesRepository, IServiceBus serviceBus)
        {
            this.gamesRepository = gamesRepository;
            this.movesRepository = movesRepository;
            this.serviceBus = serviceBus;
        }

        public override int ExecuteCommand(MovePieceCommand command)
        {
            var game = gamesRepository.GetGame(command.GameId);

            EnsureGameNotFinished(game);
    
            var movesLog = GetMovesLog(command.GameId);
            var board = CreateBoard(movesLog);

            var from = command.From.AsDomain();
            var to = command.To.AsDomain();

            EnsureIsPlayerTurn(board, movesLog, from);
            EnsureIsValidMove(board, movesLog, from, to);

            var pieceMove = new PieceMove(movesLog.NextMoveSequenceNumber(), from, to);
            
            SaveMove(command.GameId, pieceMove);
            PublishPieceMovedEvent(command.GameId, pieceMove);

            return movesLog.NextMoveSequenceNumber();
        }

        private void EnsureGameNotFinished(Game game)
        {
            if (game.IsFinished)
                throw new Exception("Cannot move pieces in finished game.");
        }

        private void SaveMove(string gameId, PieceMove pieceMove)
        {   
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

            board.ApplyMoves(movesLog.Select(m => m.Move));

            return board;
        }

        private void EnsureIsValidMove(Board board, MovesLog movesLog, Location from, Location to)
        {
            var pieceMover = new PieceMover();

            pieceMover.EnsureIsValidMove(board, movesLog, from, to);
        }

        private void PublishPieceMovedEvent(string gameId, PieceMove move)
        {
            serviceBus.Publish(new PieceMovedEvent
            {
                GameId = gameId,
                PieceMove = new PieceMoveDto
                {
                    SequenceNumber = move.SequenceNumber,
                    From = move.From.AsDto(),
                    To = move.To.AsDto()
                }
            });
        }
    }
}
