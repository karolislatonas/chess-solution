namespace Chess.Domain.Movement.Moves
{
    public abstract class MoveBase : IMove
    {
        public MoveBase(Location from, Location to)
        {
            From = from;
            To = to;
        }

        public Location From { get; }

        public Location To { get; }

        public abstract void ApplyChanges(Board board);
    }
}
