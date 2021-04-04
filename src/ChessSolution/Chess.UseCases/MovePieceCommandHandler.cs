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
using Chess.Domain.Extensions;

namespace Chess.UseCases
{
    public class MovePieceCommandHandler : CommandHandlerBase<MovePieceCommand, int>
    {
        private readonly IGamesRepository gamesRepository;
        private readonly IMovesRepository movesRepository;
        private readonly IServiceBus serviceBus;
        private readonly PieceMover pieceMover;

        public MovePieceCommandHandler(IGamesRepository gamesRepository, IMovesRepository movesRepository, IServiceBus serviceBus)
        {
            this.gamesRepository = gamesRepository;
            this.movesRepository = movesRepository;
            this.serviceBus = serviceBus;

            pieceMover = new PieceMover();
        }

        public override int ExecuteCommand(MovePieceCommand command)
        {
            var game = gamesRepository.GetGame(command.GameId);

            EnsureGameNotFinished(game);

            var movesReplayer = ApplyNewMove(command);
            var movesLog = movesReplayer.MovesLog;
            var board = movesReplayer.Board;

            SaveAndPublishLastMove(command.GameId, movesLog);
            UpdateAndNotifyIfGameFinished(game, board, movesLog);

            return movesLog.LastMove.SequenceNumber;
        }

        private MovesReplayer ApplyNewMove(MovePieceCommand command)
        {
            var movesReplayer = GetMovesReplayer(command.GameId);
            var movesLog = movesReplayer.MovesLog;
            var board = movesReplayer.Board;

            var from = command.From.AsDomain();
            var to = command.To.AsDomain();

            EnsureIsPlayerTurn(board, movesLog, from);

            var move = pieceMover.GetMove(board, movesLog, from, to);
            movesReplayer.AddMoveAndReplay(move);

            return movesReplayer;
        }

        private void SaveAndPublishLastMove(string gameId, MovesLog movesLog)
        {
            var lastMove = movesLog.LastMove;
            var pieceMove = new PieceMove(lastMove.SequenceNumber, lastMove.Move.From, lastMove.Move.To);

            SaveMove(gameId, pieceMove);

            PublishPieceMovedEvent(gameId, pieceMove);
        }

        private void UpdateAndNotifyIfGameFinished(Game game, Board board, MovesLog movesLog)
        {
            var turnsTracker = new TurnsTracker(movesLog);
            var playerToMove = turnsTracker.GetCurrentPlayerToMove();
            var opponentCanMove = pieceMover.CanPlayerMove(board, movesLog, playerToMove);

            if (opponentCanMove)
                return;

            var winner = playerToMove.GetOpposite();
            game.UpdateResultToWin(winner);
            gamesRepository.UpdateGame(game);
            PublishGameFinishedEvent(game);
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

        private MovesReplayer GetMovesReplayer(string gameId)
        {
            var movesSequenceTranslator = new MoveSequenceTranslator();

            var allMoves = movesRepository
                .GetGameMoves(gameId)
                .Select(movesSequenceTranslator.TranslateNextMove);

            var movesReplayer = MovesReplayer.CreateAndReplay(new MovesLog(allMoves));

            return movesReplayer;
        }

        private void PublishPieceMovedEvent(string gameId, PieceMove move)
        {
            serviceBus.Publish(new PieceMovedEvent
            {
                GameId = gameId,
                PieceMove = move.AsDto()
            });
        }

        private void PublishGameFinishedEvent(Game game)
        {
            serviceBus.Publish(new GameFinishedEvent
            {
                GameId = game.GameId,
                Result = game.Result.Value.AsDto()
            });
        }
    }
}
