namespace Chess.Domain.Movement.Moves
{
    public interface IMove
    {
        Location From { get; }

        Location To { get; }

        void ApplyChanges(Board board);
    }
}
