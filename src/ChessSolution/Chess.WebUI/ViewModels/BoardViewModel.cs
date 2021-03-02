using Chess.Api.Client;
using Chess.Api.DataContracts;
using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Domain.Movement.Moves;
using Chess.Domain.Pieces;
using Chess.WebUI.Translations;
using System;
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
        private readonly TurnsTracker turnsTracker;

        private string gameId;

        public event Action OnStateChanged;

        public BoardViewModel(MovementService movementService)
        {
            this.movementService = movementService;

            pieceMover = new PieceMover();
            movesReplayer = new MovesReplayer(new MovesLog());
            turnsTracker = new TurnsTracker(movesReplayer.MovesLog);
        }

        public PieceSelection SelectedPiece { get; private set; }

        public int CurrentMoveNumber => movesReplayer.CurrentMoveNumber;

        public IEnumerable<PieceMove> Moves => movesReplayer.MovesLog.Select(m => new PieceMove(m.SequenceNumber, m.Move.From, m.Move.To));

        public IPiece GetPieceAt(Location location) => movesReplayer.Board.GetPieceAt(location);

        public void SelectPieceAt(int column, int row)
        {
            var from = new Location(column, row);
            var board = movesReplayer.Board;
            var piece = board.GetPieceAt(from);

            var availableMoves = turnsTracker.IsTurnFor(piece.Color) ?
                pieceMover.GetAvailableMoves(board, movesReplayer.MovesLog, from) :
                new IMove[0];
                
            SelectedPiece = new PieceSelection(from, piece, availableMoves);
        }

        public void ClearPieceSelection()
        {
            SelectedPiece = null;
        }

        public void NextMove()
        {
            movesReplayer.ToNextMove();
            NotifyStateChanged();
        }

        public void PreviousMove()
        {
            movesReplayer.ToPreviousMove();
            NotifyStateChanged();
        }

        public void ToMove(int moveNumber)
        {
            movesReplayer.ToMove(moveNumber);
            NotifyStateChanged();
        }

        public void ToLastMove()
        {
            movesReplayer.ToLastMove();
            NotifyStateChanged();
        }

        public void ToStart()
        {
            movesReplayer.ToStart();
            NotifyStateChanged();
        }

        public bool BelongsToCurrentMove(Location location)
        {
            var current = movesReplayer.GetCurrentMove();

            if (current == null)
                return false;

            return location == current.To ||
                location == current.From;
        }

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
                ToLastMove();
                return;
            }

            //await PushMoveAsync(selectedPiece.From, to);

            var move = selectedPiece.GetMoveAt(to);
            movesReplayer.AddMove(move);

            NotifyStateChanged();
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

            var movesSequenceTranslator = new MoveSequenceTranslator();

            var moves = movesResponse
                .Moves
                .Select(m => m.AsDomain())
                .Select(movesSequenceTranslator.TranslateNextMove);

            foreach (var move in moves)
                movesReplayer.AddMove(move);
        }
     
        private void NotifyStateChanged()
        {
            OnStateChanged?.Invoke();
        }
    }
}
