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

        private string gameId;

        public BoardViewModel(MovementService movementService)
        {
            this.movementService = movementService;

            board = new Board();
        }
            
        public IPiece GetPieceAt(Location location)
        {
            return board.GetPieceAt(location);
        }

        public IEnumerable<Location> GetAvailablePieceMoves(Location location)
        {
            yield break;
        }


        public async Task MovePieceAsync(Location from, Location to)
        {
            if (from == to)
                return;

            var pieceMove = new MovePieceRequestDto
            {
                From = new LocationDto { Column = from.Column, Row = from.Row },
                To = new LocationDto { Column = to.Column, Row = to.Row }
            };

            var response = await movementService.MovePieceAsync(gameId, pieceMove);

            var move = response.Move.AsDomain();

            board.ApplyMove(move);
        }

        public async Task InitialiseAsync(string initialiseGameId)
        {
            gameId = initialiseGameId;

            await InitialiseMovesAsync();
        }

        private async Task InitialiseMovesAsync()
        {
            var movesResponse = await movementService.GetGameMovesAsync(gameId);

            var moves = movesResponse.Moves.Select(m => m.AsDomain());

            board.ApplyMoves(moves);
        }
        
    }
}
