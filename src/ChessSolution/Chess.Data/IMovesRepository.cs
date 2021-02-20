using Chess.Domain.Movement;

namespace Chess.Data
{
    public interface IMovesRepository
    {
        PieceMove[] GetGameMoves(string gameId);

        void AddMove(string gameId, PieceMove pieceMove);
    }
}
