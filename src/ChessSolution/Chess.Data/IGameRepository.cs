using Chess.Domain;

namespace Chess.Data
{
    public interface IGameRepository
    {
        void AddGame(Game game);
    }
}
