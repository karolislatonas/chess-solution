using Chess.Domain;

namespace Chess.Data
{
    public interface IGamesRepository
    {
        Game GetGame(string gameId);

        void AddGame(Game game);
    }
}
