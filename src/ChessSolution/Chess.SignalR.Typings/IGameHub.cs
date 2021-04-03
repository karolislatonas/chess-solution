namespace Chess.SignalR.Typings
{
    public interface IGameHub
    {
        void SubscribeToGameEvents(string gameId, int fromSequenceNumber);
    }
}
