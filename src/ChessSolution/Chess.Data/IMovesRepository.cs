using Chess.Domain.Movement;

namespace Chess.Data
{
    public interface IMovesRepository
    {
        PieceMove[] GetGameMoves(string gameId);

        PieceMove GetMove(string gameId, int sequenceNumber);

        void AddMove(string gameId, PieceMove pieceMove);
    }
}
