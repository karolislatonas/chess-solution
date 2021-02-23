using Chess.Api.Client;
using Chess.Api.DataContracts;
using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Domain.Pieces;
using Chess.WebUI.Translations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.WebUI.ViewModels
{
    public class BoardViewModel
    {
        private readonly MovementService movementService;
        private readonly Board board;
        private readonly PieceMover pieceMover;
        private MovesLog movesLog;

        private string gameId;

        public BoardViewModel(MovementService movementService)
        {
            this.movementService = movementService;

            board = new Board();
            pieceMover = new PieceMover(board);
        }

        public PieceSelection SelectedPiece { get; private set; }

        public PieceMove LatestMove => movesLog.LatestMove;

        public IPiece GetPieceAt(Location location) => board.GetPieceAt(location);

        public void SelectPieceAt(int column, int row)
        {
            var from = new Location(column, row);
            var piece = board.GetPieceAt(from);

            var availableMoves = pieceMover.GetAvailableMoves(from);

            SelectedPiece = new PieceSelection(from, piece, availableMoves);
        }

        public void ClearPieceSelection()
        {
            SelectedPiece = null;
        }

        public async Task MoveSelectedPieceToAsync(Location to)
        {
            var selectedPiece = SelectedPiece;
            ClearPieceSelection();

            if (selectedPiece == null)
                return;

            if (!selectedPiece.CanMoveTo(to))
                return;

            await MovePieceAsync(selectedPiece.From, to);
        }

        public async Task MovePieceAsync(Location from, Location to)
        {
            movesLog.AddMove(from, to);

            board.ApplyMove(movesLog.LatestMove);
        }

        public async Task MovePieceAsyncB(Location from, Location to)
        {
            var pieceMove = new MovePieceRequestDto
            {
                From = new LocationDto { Column = from.Column, Row = from.Row },
                To = new LocationDto { Column = to.Column, Row = to.Row }
            };

            var response = await movementService.MovePieceAsync(gameId, pieceMove);

            var move = response.Move.AsDomain();

            movesLog.AddMove(move.From, move.To);
            board.ApplyMove(move);
        }

        public async Task InitialiseAsync(string initialiseGameId)
        {
            gameId = initialiseGameId;

            await InitialiseMovesAsync();
        }

        private async Task InitialiseMovesAsync()
        {
            //var movesResponse = await movementService.GetGameMovesAsync(gameId);

            //movesLog = new MovesLog(movesResponse.Moves.Select(m => m.AsDomain()));
            movesLog = new MovesLog();

            board.ApplyMoves(movesLog);
        }
        
    }
}
