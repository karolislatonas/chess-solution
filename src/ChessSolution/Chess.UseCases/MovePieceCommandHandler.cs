using Chess.Data;
using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Messages.Commands;
using Chess.Messages.DomainTranslation;

namespace Chess.UseCases
{
    public class MovePieceCommandHandler : CommandHandlerBase<MovePieceCommand>
    {
        private readonly IMovesRepository movesRepository;

        public MovePieceCommandHandler(IMovesRepository movesRepository)
        {
            this.movesRepository = movesRepository;
        }

        public override void ExecuteCommand(MovePieceCommand command)
        {
            var movesLog = GetMovesLog(command.GameId);

            var board = CreateBoard(movesLog);

            var pieceMover = new PieceMover(board, movesLog);

            var pieceMove = pieceMover.GetPieceMove(command.From.ToDomain(), command.To.ToDomain());

            movesRepository.AddMove(command.GameId, pieceMove);
        }

        private MovesLog GetMovesLog(string gameId)
        {
            var allMoves = movesRepository.GetGameMoves(gameId);

            return new MovesLog(allMoves);
        }

        private Board CreateBoard(MovesLog movesLog)
        {
            var board = new Board();
            board.ApplyMoves(movesLog.Moves);

            return board;
        }
    }
}
