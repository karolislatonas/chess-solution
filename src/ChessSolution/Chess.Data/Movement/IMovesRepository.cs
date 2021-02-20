using Chess.Data.Movement;

namespace Chess.Data
{
    public interface IMovesRepository
    {
        PieceMoveDto[] GetGameMoves(int gameId);

        void AddMove(PieceMoveDto pieceMove);
    }
}
