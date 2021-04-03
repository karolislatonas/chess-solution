using Chess.Domain;

namespace Chess.Data
{
    public interface IGamesRepository
    {
        Game GetGame(string gameId);

        void UpdateGame(Game game);

        void AddGame(Game game);
    }
}
