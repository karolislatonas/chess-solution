using Chess.Api.Client;
using Chess.Api.DataContracts;
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
        private readonly PieceMover pieceMover;
        private readonly MovementService movementService;
        private readonly MovesReplayer movesReplayer;

        private string gameId;

        public BoardViewModel(MovementService movementService)
        {
            this.movementService = movementService;

            pieceMover = new PieceMover();
            movesReplayer = new MovesReplayer(new MovesLog());
        }

        public PieceSelection SelectedPiece { get; private set; }

        public int CurrentMoveNumber => movesReplayer.CurrentMoveNumber;

        public IEnumerable<PieceMove> Moves => movesReplayer.MovesLog;

        public IPiece GetPieceAt(Location location) => movesReplayer.Board.GetPieceAt(location);

        public void SelectPieceAt(int column, int row)
        {
            var from = new Location(column, row);
            var board = movesReplayer.Board;
            var piece = board.GetPieceAt(from);

            var availableMoves = pieceMover.GetAvailableMoves(board, from);

            SelectedPiece = new PieceSelection(from, piece, availableMoves);
        }

        public void ClearPieceSelection()
        {
            SelectedPiece = null;
        }

        public void NextMove() => movesReplayer.ToNextMove();

        public void PreviousMove() => movesReplayer.ToPreviousMove();

        public void ToMove(int moveNumber) => movesReplayer.ToMove(moveNumber);

        public void ToLastMove() => movesReplayer.ToLastMove();

        public void ToStart() => movesReplayer.ToStart();

        public async Task MoveSelectedPieceToAsync(Location to)
        {
            var selectedPiece = SelectedPiece;
            ClearPieceSelection();

            if (selectedPiece == null)
                return;

            if (!selectedPiece.CanMoveTo(to))
                return;

            if (!movesReplayer.IsAtLastMove)
            {
                movesReplayer.ToLastMove();
                return;
            }

            

            //await PushMoveAsync(selectedPiece.From, to);

            movesReplayer.AddMove(selectedPiece.From, to);
        }

        public async Task PushMoveAsync(Location from, Location to)
        {
            var pieceMove = new MovePieceRequestDto
            {
                From = new LocationDto { Column = from.Column, Row = from.Row },
                To = new LocationDto { Column = to.Column, Row = to.Row }
            };

            await movementService.MovePieceAsync(gameId, pieceMove);
        }

        public async Task InitialiseAsync(string initialiseGameId)
        {
            gameId = initialiseGameId;

            //await InitialiseMovesAsync();
        }

        private async Task InitialiseMovesAsync()
        {
            var movesResponse = await movementService.GetGameMovesAsync(gameId);

            var moves = movesResponse.Moves.Select(m => m.AsDomain());

            foreach (var move in moves)
                movesReplayer.AddMove(move);
        }
        
    }
}
