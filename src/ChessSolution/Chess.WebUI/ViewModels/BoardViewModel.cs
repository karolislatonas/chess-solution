using Chess.Api.Client;
using Chess.Api.DataContracts;
using Chess.Domain;
using Chess.Domain.Movement;
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

        public IEnumerable<PieceLocation> Pieces => board.GetAllPieces();

        public async Task MovePiece(int fromColumn, int fromRow, int toColumn, int toRow)
        {
            var pieceMove = new PieceMoveDto
            {
                From = new LocationDto { Column = fromColumn, Row = fromRow },
                To = new LocationDto { Column = toColumn, Row = toRow }
            };

            await movementService.MovePieceAsync(gameId, pieceMove);
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
