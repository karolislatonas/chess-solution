using Chess.Api.Client;
using Chess.Api.DataContracts;
using Chess.Domain;
using Chess.Domain.Movement;
using Chess.Domain.Movement.Moves;
using Chess.Domain.Pieces;
using Chess.Api.Client.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chess.Messages.Events;
using Chess.Api.Client.Subscription.Subscribers;
using Chess.WebUI.Translations;

namespace Chess.WebUI.ViewModels
{
    public class BoardViewModel : IDisposable
    {
        private readonly MovementService movementService;
        private readonly ISubscriptionProvider subscriptionProvider;
        private readonly PieceMover pieceMover;
        private readonly MovesReplayer movesReplayer;
        private readonly TurnsTracker turnsTracker;
        private readonly MoveSequenceTranslator movesSequenceTranslator;

        private string gameId;
        private ISubscriber movesSubscriber;

        public event Action OnStateChanged;

        public BoardViewModel(MovementService movementService, ISubscriptionProvider subscriptionProvider)
        {
            this.movementService = movementService;
            this.subscriptionProvider = subscriptionProvider;

            pieceMover = new PieceMover();
            movesSequenceTranslator = new MoveSequenceTranslator();
            movesReplayer = new MovesReplayer(new MovesLog());
            turnsTracker = new TurnsTracker(movesReplayer.MovesLog);
        }

        public PieceSelection SelectedPiece { get; private set; }

        public int CurrentMoveNumber => movesReplayer.CurrentMoveNumber;

        public BoardDetails BoardDetails => movesReplayer.Board.BoardDetails;

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

            if (!movesReplayer.IsAtLastMove)
            {
                ToLastMove();
                return;
            }

            if (selectedPiece == null)
                return;

            if (!selectedPiece.CanMoveTo(to))
                return;

            await PushMoveAsync(selectedPiece.From, to);
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

            await InitialiseMovesAsync();
            ToLastMove();

            await SubscriberForNewMovesAsync();
        }
        private async Task InitialiseMovesAsync()
        {
            var movesResponse = await movementService.GetGameMovesAsync(gameId);

            var moves = movesResponse
                .Moves
                .Select(m => m.AsDomain())
                .Select(movesSequenceTranslator.TranslateNextMove);

            movesReplayer.AddMoves(moves);
        }

        private async Task SubscriberForNewMovesAsync()
        {
            var subscriberFromSequence = movesReplayer.MovesLog.NextMoveSequenceNumber();

            movesSubscriber = await subscriptionProvider.SubscribeAsync(gameId, subscriberFromSequence, OnPieceMoved);
        }

        private void OnPieceMoved(PieceMovedEvent pieceMovedEvent)
        {
            var from = new Location(pieceMovedEvent.From.Column, pieceMovedEvent.From.Row);
            var to = new Location(pieceMovedEvent.To.Column, pieceMovedEvent.To.Row);

            var move = movesSequenceTranslator.TranslateNextMove(from, to);

            var shouldUpdateToLatest = movesReplayer.IsAtLastMove;

            movesReplayer.AddMove(move);

            if(shouldUpdateToLatest)
                movesReplayer.ToLastMove();

            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnStateChanged?.Invoke();
        }

        public void Dispose()
        {
            movesSubscriber?.Dispose();
        }
    }
}
