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
using Chess.Domain.Extensions;
using Chess.Shared.DataContracts;
using Chess.Shared.DataContracts.Translations;

namespace Chess.WebUI.ViewModels
{
    public class BoardViewModel : IAsyncDisposable
    {
        private readonly GameService gameService;
        private readonly MovementService movementService;
        private readonly ISubscriptionProvider subscriptionProvider;
        private readonly PieceMover pieceMover;
        private readonly MovesReplayer movesReplayer;
        private readonly TurnsTracker turnsTracker;
        private readonly MoveSequenceTranslator movesSequenceTranslator;
        
        private string gameId;
        private string whitePlayerId;
        private string blackPlayerId;

        private ISubscriber movesSubscriber;
        private ChessColor viewPerspective;

        public event Action OnStateChanged;

        public BoardViewModel(GameService gameService, MovementService movementService, ISubscriptionProvider subscriptionProvider)
        {
            this.gameService = gameService;
            this.movementService = movementService;
            this.subscriptionProvider = subscriptionProvider;

            viewPerspective = ChessColor.White;

            pieceMover = new PieceMover();
            movesSequenceTranslator = new MoveSequenceTranslator();
            movesReplayer = new MovesReplayer(new MovesLog());
            turnsTracker = new TurnsTracker(movesReplayer.MovesLog);
        }

        public PieceSelection SelectedPiece { get; private set; }

        public int CurrentMoveNumber => movesReplayer.CurrentMoveNumber;

        public BoardDetails BoardDetails => movesReplayer.Board.BoardDetails;

        public bool IsGameFinished => Result.HasValue;

        public GameResult? Result { get; private set; }

        public IEnumerable<PieceMove> Moves => movesReplayer.MovesLog.Select(m => new PieceMove(m.SequenceNumber, m.Move.From, m.Move.To));

        public IPiece GetPieceAt(Location location) => movesReplayer.Board.GetPieceAt(location);

        public Location GetLocationAtIndex(int columnNumber, int rowNumber)
        {
            if (viewPerspective == ChessColor.Black)
            {
                var columnFromBlack = BoardDetails.TotalColumns - columnNumber + 1;
                var rowFromBlack = BoardDetails.TotalRows - rowNumber + 1;

                return new Location(columnFromBlack, rowFromBlack);
            }

            return new Location(columnNumber, rowNumber);
        }

        public void SelectPieceAt(Location location)
        {
            var board = movesReplayer.Board;
            var piece = board.GetPieceAt(location);

            var availableMoves = turnsTracker.IsTurnFor(piece.Color) ?
                pieceMover.GetAvailableMoves(board, movesReplayer.MovesLog, location) :
                Array.Empty<IMove>();
                
            SelectedPiece = new PieceSelection(location, piece, availableMoves);
        }

        public async Task Resign()
        {
            var playerToResignId = GetPlayerIdByColor(viewPerspective);

            await gameService.Resign(gameId, playerToResignId);
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

        public void SwitchViewPerspective()
        {
            viewPerspective = viewPerspective.GetOpposite();
        }

        public async Task InitialiseAsync(string initialiseGameId)
        {
            gameId = initialiseGameId;

            await InitialiseGameAsync();
            await InitialiseMovesAsync();
            ToLastMove();

            await SubscribeForNewMovesAsync();
        }

        private async Task InitialiseGameAsync()
        {
            var gameResponse = await gameService.GetGameAsync(gameId);

            whitePlayerId = gameResponse.WhitePlayerId;
            blackPlayerId = gameResponse.BlackPlayerId;
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

        private async Task SubscribeForNewMovesAsync()
        {
            var subscriberFromSequence = movesReplayer.MovesLog.NextMoveSequenceNumber();

            movesSubscriber = await subscriptionProvider.SubscribeAsync(gameId, subscriberFromSequence, OnPieceMoved, OnGameFinished);
        }

        private void OnPieceMoved(PieceMovedEvent pieceMovedEvent)
        {
            var from = pieceMovedEvent.PieceMove.From.AsDomain();
            var to = pieceMovedEvent.PieceMove.To.AsDomain();

            var move = movesSequenceTranslator.TranslateNextMove(from, to);

            var shouldUpdateToLatest = movesReplayer.IsAtLastMove;

            movesReplayer.AddMove(move);

            if(shouldUpdateToLatest)
                movesReplayer.ToLastMove();

            NotifyStateChanged();
        }

        private void OnGameFinished(GameFinishedEvent gameFinishedEvent)
        {
            Result = gameFinishedEvent.Result.AsDomain();

            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnStateChanged?.Invoke();
        }

        private string GetPlayerIdByColor(ChessColor color)
        {
            return color switch
            {
                ChessColor.White => whitePlayerId,
                ChessColor.Black => blackPlayerId,
                _ => throw new Exception($"Failed to handle color:{color} when selecting playerId by color")
            };
        }

        public async ValueTask DisposeAsync()
        {
            var disposeTask = movesSubscriber?.DisposeAsync();

            if (disposeTask.HasValue)
                await disposeTask.Value;       
        }
    }
}
