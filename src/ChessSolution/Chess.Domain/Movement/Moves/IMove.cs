namespace Chess.Domain.Movement.Moves
{
    public interface IMove
    {
        void ApplyChanges(Board board);
    }
}
