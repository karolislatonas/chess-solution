namespace Chess.SignalR.Typings
{
    public interface IMoveHub
    {
        void SubscribeToMoves(string gameId);

        void SubscribeToMovesFrom(string gameId, int fromSequenceNumber);
    }
}
