namespace Chess.Domain
{
    public class Game
    {
        public Game(string id)
        {
            GameId = id;
        }

        public string GameId { get; }
    }
}
