using Chess.Data;
using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Messages.Commands;
using Chess.Messages.DomainTranslation;

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

            EnsureIsValidMove(from, to, board);

            var pieceMove = movesLog.GetNextMove(from, to);

            movesRepository.AddMove(command.GameId, pieceMove);

            return pieceMove.SequenceNumber;
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

        private void EnsureIsValidMove(Location from, Location to, Board board)
        {
            var pieceMover = new PieceMover(board);

            pieceMover.EnsureIsValidMove(from, to);
        }
    }
}
