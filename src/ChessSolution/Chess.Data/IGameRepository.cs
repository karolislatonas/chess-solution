using Chess.Domain;

namespace Chess.Data
{
    public interface IGameRepository
    {
        Game GetGame(string gameId);

        void AddGame(Game game);
    }
}
