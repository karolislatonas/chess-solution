using Chess.Data;
using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Messages.Commands;
using Chess.Messages.DomainTranslation;
using System;

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
            EnsureIsValidMove(board, from, to);

            movesLog.AddMove(from, to);
            var latestMove = movesLog.LatestMove;

            movesRepository.AddMove(command.GameId, latestMove);

            return latestMove.SequenceNumber;
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
            var allMoves = movesRepository.GetGameMoves(gameId);

            return new MovesLog(allMoves);
        }

        private Board CreateBoard(MovesLog movesLog)
        {
            var board = new Board();
            board.ApplyMoves(movesLog);

            return board;
        }

        private void EnsureIsValidMove(Board board, Location from, Location to)
        {
            var pieceMover = new PieceMover();

            pieceMover.EnsureIsValidMove(board, from, to);
        }
    }
}
