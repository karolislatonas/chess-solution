using Chess.Domain.Movement;

namespace Chess.Data
{
    public interface IMovesRepository
    {
        PieceMove[] GetGameMoves(string gameId);

        PieceMove GetMove(string gameId, int sequenceNumber);

        PieceMove[] GetMovesFromSequence(string gameId, int fromSequenceNumber);

        void AddMove(string gameId, PieceMove pieceMove);
    }
}
